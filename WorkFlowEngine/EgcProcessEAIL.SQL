USE [EGCApps]
GO
/****** Object:  StoredProcedure [dbo].[EGC_ProcessEmails]    Script Date: 25-07-2024 20:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





ALTER PROCEDURE [dbo].[EGC_ProcessEmails]
(
	 @RedirectSiteUrl		NVARCHAR(100),
	 @VPESiteUrl			NVARCHAR(100),
	 @EIMSiteUrl			NVARCHAR(100)
 )

AS BEGIN
	SET XACT_ABORT ON;

	SET NOCOUNT ON;

	DECLARE @error INT,@Subject	NVARCHAR(400),
	@siteUrl NVARCHAR(100)= @RedirectSiteUrl,
	@PageURL NVARCHAR(200)='/redirect.aspx?frommode=NC&ID=',
	@Importance NVARCHAR(20) = 'Normal',
	@EGCLogoUrl NVARCHAR(100)= 'cid:egclogo.png', 
	@BodyHeader NVARCHAR(MAX) ='' ,
	@BodyFooter NVARCHAR(MAX) = ''

	SET @BodyHeader = '<div style="height:100px;background-color:#023C75;margin-bottom:10px;"><img src="' + @EGCLogoUrl + '" style="height:10px;"></div><div>'
	SET @BodyFooter = '</div><div style="margin-top:10px;font-size:11pt;font-family:calibri;"><hr><b>Confidentiality Notice</b><hr>This email, including any attachments, is for the sole use of the intended recipient(s) and may contain confidential and privileged information. '+
					'Any unauthorized review, use, disclosure or distribution is prohibited. If you are not the intended recipient, please contact the sender by phone or email and delete the original message and all copies.</div>'			

	DECLARE @xmlACDInfo NVARCHAR(MAX) = '',@bodyACDInfo NVARCHAR(MAX) = '', @CreatedByUserName NVARCHAR(100) = ''
	DECLARE @TEMPACDInfo AS TABLE(ACDID INT,Company NVARCHAR(200),Division NVARCHAR(200),EGCPartNo NVARCHAR(100),ManufacturerName NVARCHAR(250),ManufacturerPartNo NVARCHAR(100), CustPartNo NVARCHAR(MAX), Comments NVARCHAR(MAX))
	DECLARE @TEMPSumMinMax AS TABLE(ACDID INT,Company NVARCHAR(200),Division NVARCHAR(200),EGCPartNo NVARCHAR(100), CustPartNo NVARCHAR(MAX),CMin INT,CMax INT,NMin INT,NMax INT,PartType INT)
	DECLARE @TEMPMarginInfo	AS TABLE(CONO INT,CompanyName NVARCHAR(100),Division NVARCHAR(100),CustGrp NVARCHAR(100),OrderNo NVARCHAR(20),Suf INT,EGCPart NVARCHAR(100),ProductType NVARCHAR(10),Comment NVARCHAR(MAX))
	DECLARE @xmlMarginInfo NVARCHAR(MAX) = '',@bodyMarginInfo NVARCHAR(MAX) = ''

	DECLARE @SrNo INT, @EmailTo NVARCHAR(MAX),@EmailCC NVARCHAR(MAX),@EmailBCC NVARCHAR(MAX),
	@EmailType INT,@EmailAttachment NVARCHAR(MAX)='',@ReplyTo NVARCHAR(MAX),@EmailFrom NVARCHAR(MAX),@EmailBody NVARCHAR(MAX),@RecipientType NVARCHAR(10),
	@AttachmentRoot NVARCHAR(200)
	
	DECLARE @IDVal INT, @StrBody NVARCHAR(MAX),@LineTable NVARCHAR(MAX)='',@Calendarquery NVARCHAR(MAX)='',@AgendaCount INT= 0,@TaskCount INT = 0,@IParticipantID INT,
	@ParticipantEmailID	NVARCHAR(1000),@ParticipantUserName NVARCHAR(1000),@HeadedByEmailID NVARCHAR(100),@AvailabilityReasons NVARCHAR(1000),@CreatedByEmailID NVARCHAR(100),
	@ParticipantAvailabilityTable NVARCHAR(MAX)='',@ACDType NVARCHAR(10),@CONO INT,@Division INT,@CustomerGrp INT,@VSType NVARCHAR(10),@CrtUNm NVARCHAR(200)=''
	DECLARE @internalAttendee NVARCHAR(MAX) ='', @externalAttendee NVARCHAR(MAX)='',@customerAttendee NVARCHAR(MAX)=''
	DECLARE @SupplierPortalEnabled INT = 0

	DECLARE @PONum NVARCHAR(100)='',@ShipToAdress NVARCHAR(MAX) ='',@HeaderIssueTp NVARCHAR(100)=''

	DECLARE @VSSecondLevelApporvalCono NVARCHAR(200)
	SET @VSSecondLevelApporvalCono =',1,2,4,6,9,1002,3002,2001,2002,3001,3010,3011,3001,4002,7008,3016,7001,3013,'
	
 
	DECLARE Cursor_ProcessEmails CURSOR FOR 
	SELECT IEmailSrNo,EmailType,EmailTo,EmailCC,EmailBCC,IDVal,EmailAttachment,EmailSubject,EmailBody,RecipientType,IParticipantID,FromAddress
	FROM EmailNotificationLog E
	WHERE ISNULL(ISProcessed,0) = 0
	   
	OPEN Cursor_ProcessEmails 	 
	  
	FETCH NEXT FROM Cursor_ProcessEmails  
	INTO @SrNo,@EmailType,@EmailTo,@EmailCC,@EmailBCC,@IDVal,@EmailAttachment,@Subject,@EmailBody,@RecipientType,@IParticipantID,@EmailFrom
		   
	WHILE @@FETCH_STATUS=0 BEGIN 	
		SET @AttachmentRoot=''
		SET @Importance = 'Normal'
		-- Get data to generate email body for NC Emails
		IF @EmailType IN (1,2,3,4,5,6)
		BEGIN
			SET @Calendarquery =''
			SET @PageURL ='/redirect.aspx?frommode=NC&ID='

			SELECT DISTINCT CONVERT(NVARCHAR(10),H.PoNum) + '-' + CONVERT(NVARCHAR(10),H.PoSuf) AS PoNumSuf, CONVERT(NVARCHAR(100),L.LineNum) AS LineNum,
			L.PartNumber,ISNULL(PartDesc,'') AS PartDesc,
			STUFF((SELECT ', ' + NR.ReferenceText 
			FROM [NCLinesRootCause] R WITH(NOLOCK)
			JOIN NCReferences NR
			ON R.IRootCauseID = NR.ReferenceKey
			AND ReferenceTypeKey=5  
			WHERE R.INonConformanceID = H.INonConformanceID
			AND L.LineNum = R.LineNum FOR XML PATH ('')), 1, 1, '') AS RootCauses,
			H.LastUpdatedUserName AS LastUpdatedByUserName,
			ISNULL((SELECT TOP 1 CommentText 
			FROM  NCComments NC with (Nolock)
			WHERE NC.INonConformanceID = H.INonConformanceID
			AND L.LineNum = NC.LineNum
			ORDER BY NC.CommentDate DESC),'') AS LatestComments,
			ISNULL(V.VendorName,'') AS VendorName,
			ISNULL(H.Buyer,'') AS Buyer,
			CONVERT(NVARCHAR(10),ISNULL(H.PoNum,'')) AS PoNum,
			ISNULL(H.IsPOKnown,0) AS IsPOKnown,
			ISNULL(H.ShipTo,'') AS ShipTo
			INTO #NCDetails 
			FROM NCHeader H 
			JOIN NCLines L WITH(NOLOCK)
			ON H.INonConformanceID = L.INonConformanceID
			LEFT OUTER JOIN EGCAPPSDWHS.dbo.[DBW-SV11-Vendor] V WITH(NOLOCK)
			ON H.CONO = V.CompanyNumber
			AND H.VendorNumber = V.VendorNumber
			WHERE H.INonConformanceID = @IDVal	
		
			DECLARE @EmailUserName NVARCHAR(100)=''
			SELECT DISTINCT  @EmailUserName = LastUpdatedByUserName FROM #NCDetails
		END

		IF @EmailType = 1 --NC Assigned to EGC User
		BEGIN
			SET @ReplyTo = @EmailBCC
			SET @EmailFrom = @EmailBCC
			SET @Subject = 'EGC NonConformance Request # '+ CONVERT(NVARCHAR(100),@IDVal) +' is assigned to you'				

			SELECT DISTINCT @StrBody = '<font style="font-size:11pt;font-family:calibri;">Hi,<br><br> NonConformance Request# ' + CONVERT(NVARCHAR(100),@IDVal) + ' is assigned to you by  ' + N.LastUpdatedByUserName + '.' +
			'<br><br>PO#:' + N.PoNumSuf + '<br><br><table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3"><tr><td><b>Line#</b></td><td><b>Part#</b></td><td>' +
			'<b>Part description</b></td><td><b>Root causes</b></td></tr>'
			FROM #NCDetails N

			SET @LineTable=''
			SELECT @LineTable = @LineTable + '<tr><td valign="top">' + CASE N.LineNum  WHEN '0' THEN 'Header' ELSE N.LineNum END +'</td><td valign="top">'+ N.PartNumber  
			+'</td><td valign="top">'+ N.PartDesc  
			+'</td><td valign="top">'+ N.RootCauses  
			+'</td></tr>'
			FROM #NCDetails N
				
			SET @StrBody = @StrBody + @LineTable +'</table><br>Please visit Module apps to check the NC and provide your updates using the following link.'
			+ '<br><br><a href="' + @siteUrl + @PageURL + CONVERT(NVARCHAR(100),@IDVal) +'">Open NC</a><br><br>Thanks<br>EGC Team</font>'
				
		END
		ELSE IF @EmailType = 2 --NC Assigned to Vendor
		BEGIN

			SELECT @PONum= CASE N.IsPOKnown WHEN 0 THEN N.PoNum ELSE 'Not Identified' END,
			@ShipToAdress = CASE N.IsPOKnown WHEN 0 THEN 'EGC Ship To Location : '+ N.ShipTo ELSE '' END,
			@HeaderIssueTp = N.RootCauses
			FROM #NCDetails N WHERE N.LineNum='0'

			SET @ReplyTo = @EmailBCC
			SET @EmailFrom = @EmailBCC
			SET @Subject = 'Non Conformance identified by EGC – NC# '+ CONVERT(NVARCHAR(100),@IDVal)
						
			SELECT DISTINCT @StrBody = '<font style="font-size:11pt;font-family:calibri;">Hi,<br><br> Please note that a new non-conformance has been '+
			'identified by EGC which needs attention from '+ N.VendorName +'.<br>Below are the details of non-conformance identified. '+
			'Please work with EGC Buyer '+ N.Buyer +' to provide a resolution as well as to identify appropriate corrective action plan.<br><br>'+
			'<table style="font-size:11pt;font-family:calibri;width:90%" border="1" cellspacing="0" cellpadding="3"><tr><td colspan="2" style="width:30%">'+
			'<b>EGC PO Number : '+ @PONum +'</b></td><td colspan="2" style="width:60%"><b>'+ @ShipToAdress +'</b></td></tr><tr><td style="width:15%">'+
			'<b>Line#</b></td><td style="width:30%"><b>Part#</b></td><td style="width:25%"><b>Part description</b></td><td style="width:20%"><b>Issue Identified</b></td></tr>'
			FROM #NCDetails N

			SET @LineTable=''

			SELECT @LineTable = @LineTable + '<tr><td valign="top">' + N.LineNum+'</td>'+
			'<td valign="top">'+ N.PartNumber+ '</td><td valign="top">'+ N.PartDesc +'</td><td valign="top">'+ N.RootCauses +'</td></tr>'
			FROM #NCDetails N
			WHERE N.LineNum<>'0'

			SET @StrBody = @StrBody + @LineTable + '</table><br>EGC PO Number : '+@PONum+'<br>Issue Identified : '+@HeaderIssueTp+'<br><br>Regards,<br>EGC Purchasing Team</font>'

		END
		ELSE IF @EmailType = 3 --NC Resolved
		BEGIN

			SET @ReplyTo = @EmailBCC
			SET @EmailFrom = @EmailBCC
			SET @Subject = 'EGC NonConformance Request #' +CONVERT(NVARCHAR(100),@IDVal) + ' is marked resolved by ' + @EmailUserName 

			SELECT DISTINCT @StrBody = '<font style="font-size:11pt;font-family:calibri;">Hi,<br><br> NonConformance Request# ' + CONVERT(NVARCHAR(100),@IDVal) + ' is marked resolved by  ' + @EmailUserName + '.' +
			'<br><br>PO#:' + N.PoNumSuf + '<br><br><table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3"><tr><td><b>Line#</b></td><td><b>Part#</b></td><td>' +
			'<b>Part description</b></td><td><b>Root causes</b></td><td><b>Comments</b></td></tr>'
			FROM #NCDetails	N								

			SET @LineTable=''
			SELECT @LineTable = @LineTable + '<tr><td valign="top">' + CASE N.LineNum  WHEN '0' THEN 'Header' ELSE N.LineNum END  +'</td><td valign="top">'+ N.PartNumber  
			+'</td><td valign="top">'+ N.PartDesc  
			+'</td><td valign="top">'+ N.RootCauses 
			+'</td><td valign="top">'+ N.LatestComments  
			+'</td></tr>'				
			FROM #NCDetails N

			SET @StrBody = @StrBody + @LineTable +'</table><br>Please visit Module apps to check the NC and provide your updates using the following link.'
			+'<br><br><a href="' + @siteUrl+ @PageURL + CONVERT(NVARCHAR(100),@IDVal) +'">Open NC</a><br><br>Thanks<br>EGC Team</font>'

		END
		ELSE IF @EmailType = 4 --NC Request for Clarification
		BEGIN

			SET @ReplyTo = @EmailBCC
			SET @EmailFrom = @EmailBCC
			SET @Subject = 'EGC NonConformance Request #' +CONVERT(NVARCHAR(100),@IDVal) + ' is requested to clarify by ' + @EmailUserName 
											 
			SELECT DISTINCT @StrBody = '<font style="font-size:11pt;font-family:calibri;">Hi,<br><br> NonConformance Request# ' + CONVERT(NVARCHAR(100),@IDVal) + ' is requested to clarify by ' + @EmailUserName + '.' +
			'<br><br>PO#:' + N.PoNumSuf + '<br><br><table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3"><tr><td><b>Line#</b></td><td><b>Part#</b></td><td>' +
			'<b>Part description</b></td><td><b>Root causes</b></td><td><b>Comments</b></td></tr>'	
			FROM #NCDetails	N							

			SET @LineTable=''
			SELECT @LineTable = @LineTable + '<tr><td valign="top">' + CASE N.LineNum  WHEN '0' THEN 'Header' ELSE N.LineNum END  +'</td><td valign="top">'+ N.PartNumber  
			+'</td><td valign="top">'+ N.PartDesc  
			+'</td><td valign="top">'+ N.RootCauses  
			+'</td><td valign="top">'+ N.LatestComments  
			+'</td></tr>'
			FROM #NCDetails N

			SET @StrBody = @StrBody + @LineTable  +'</table><br>Please visit Module apps to check the NC and provide your updates using the following link.'
			+'<br><br><a href="' + @siteUrl+ @PageURL + CONVERT(NVARCHAR(100),@IDVal) +'">Open NC</a><br><br>Thanks<br>EGC Team</font>'

		END
		ELSE IF @EmailType = 5 --NC Reopen

		BEGIN

			SET @ReplyTo = @EmailBCC
			SET @EmailFrom = @EmailBCC
			SET @Subject = 'EGC NonConformance Request #' +CONVERT(NVARCHAR(100),@IDVal) + ' is reopened by ' + @EmailUserName 

			SELECT DISTINCT @StrBody = '<font style="font-size:11pt;font-family:calibri;">Hi,<br><br> NonConformance Request# ' + CONVERT(NVARCHAR(100),@IDVal) + ' is reopened by  ' + @EmailUserName + '.' +
			'<br><br>PO#:' + N.PoNumSuf + '<br><br><table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3"><tr><td><b>Line#</b></td><td><b>Part#</b></td><td>' +
			'<b>Part description</b></td><td><b>Root causes</b></td><td><b>Comments</b></td></tr>'
			FROM #NCDetails N

			SET @LineTable=''
			SELECT @LineTable = @LineTable + '<tr><td valign="top">' + CASE N.LineNum  WHEN '0' THEN 'Header' ELSE N.LineNum END +'</td><td valign="top">'+ N.PartNumber  
			+'</td><td valign="top">'+ N.PartDesc  
			+'</td><td valign="top">'+ N.RootCauses  
			+'</td><td valign="top">'+ N.LatestComments  
			+'</td></tr>'				
			FROM #NCDetails N

			SET @StrBody = @StrBody + @LineTable + '</table><br>Please visit Module apps to check the NC and provide your updates using the following link.'
			+'<br><br><a href="' + @siteUrl+ @PageURL + CONVERT(NVARCHAR(100),@IDVal) +'">Open NC</a><br><br>Thanks<br>EGC Team</font>'

		END
		ELSE IF @EmailType = 6 --NC Clarified
		BEGIN

			SET @ReplyTo = @EmailBCC
			SET @EmailFrom = @EmailBCC
			SELECT @Subject = 'EGC NonConformance Request #' +CONVERT(NVARCHAR(100),@IDVal) + ' is clarified by ' + @EmailUserName + ' and has been assigned to you'
			
			SELECT DISTINCT @StrBody = '<font style="font-size:11pt;font-family:calibri;">Hi,<br><br> NonConformance Request# ' + CONVERT(NVARCHAR(100),@IDVal) + ' is clarified by  ' + @EmailUserName + '.' +
			'<br><br>PO#:' + N.PoNumSuf + '<br><br><table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3"><tr><td><b>Line#</b></td><td><b>Part#</b></td><td>' +
			'<b>Part description</b></td><td><b>Root causes</b></td><td><b>Comments</b></td></tr>'
			FROM #NCDetails N		
					
			SET @LineTable=''
			SELECT @LineTable = @LineTable + '<tr><td valign="top">' + CASE N.LineNum  WHEN '0' THEN 'Header' ELSE N.LineNum END +'</td><td valign="top">'+ N.PartNumber  
			+'</td><td valign="top">'+ N.PartDesc  
			+'</td><td valign="top">'+ N.RootCauses  
			+'</td><td valign="top">'+ N.LatestComments  
			+'</td></tr>'				
			FROM #NCDetails N

			SET @StrBody = @StrBody + @LineTable + '</table><br>Please visit Module apps to check the NC and provide your updates using the following link.'
			+'<br><br><a href="' + @siteUrl+  @PageURL + CONVERT(NVARCHAR(100),@IDVal) +'">Open NC</a><br><br>Thanks<br>EGC Team</font>'				
		END
		ELSE IF @EmailType = 7 -- ACD Customer Batch Creation
		BEGIN	
			SET @Calendarquery =''
		
			SET @ReplyTo = NULL
			SET @EmailFrom = 'noreply@theegc.com'
							
			SELECT DISTINCT B.CONO, C.CoName AS CompanyName, B.CustGrp, C.CustomerGroup, B.Division, C.DivName AS DivDesc, IBatchID
			INTO #TEMP_Batch_Data
			FROM ACDBatchHeader B
			JOIN EGCAPPSDWHS.[dbo].[DBW-MST01-MasterCustomerInfo] C WITH (NOLOCK)
			ON B.CONO = C.CoNum
			AND B.CustGrp = C.CustGroupID
			AND B.Division = C.DivNum
			WHERE IBatchID=@IDVal

			SELECT @StrBody = '<font style="font-size:11pt;font-family:calibri;">Hi,<br><br> Batch# ' +CONVERT(NVARCHAR(100),@IDVal)+' has been created for the Division : '+ DiVDesc 
			+'.<br><br>Please find the attached Customer Buyer Report.<br><br>Thanks<br>EGC Team</font>' FROM #TEMP_Batch_Data
					
			DECLARE @ACDCustomerBatchUserID NVARCHAR(100)
			SELECT DISTINCT @ACDCustomerBatchUserID =SiteMGR
			FROM ACDBatchHeader BH WITH(NOLOCK)
			JOIN EGCAPPSDWHS.[dbo].[mstrSA006Site_Plant_Information] SPI WITH(NOLOCK)
			ON BH.CONO=SPI.Cono
			AND BH.Division=SPI.DivNo
			WHERE IBatchID=@IDVal

			DECLARE @Object AS INT;
			DECLARE @ResponseText AS VARCHAR(512);
			DECLARE @WebServiceParam AS VARCHAR(250);
			DECLARE @APIUrl NVARCHAR(250)=''
			DECLARE @WebServiceCallUrl NVARCHAR(512)=''

			SELECT @APIUrl = APISiteUrl +'Master/api/GenerateACDBuyerReport?' FROM MasterEmailType WHERE IEmailType=7
			SELECT @WebServiceParam='BatchID=' + CONVERT(NVARCHAR(100),IBatchID) +'&CustGroup='  + CONVERT(NVARCHAR(10),CustGrp) 
			+ '&Division='  + CONVERT(NVARCHAR(100),DiVDesc) FROM #TEMP_Batch_Data
			
			SET @WebServiceCallUrl=@APIUrl + @WebServiceParam

			Exec sp_OACreate 'MSXML2.XMLHTTP', @Object OUT;
			Exec sp_OAMethod @Object, 'open', NULL, 'get',@WebServiceCallUrl,'false'
			Exec sp_OAMethod @Object, 'send'
			Exec sp_OAMethod @Object, 'responseText', @ResponseText OUTPUT
			Exec sp_OADestroy @Object 
					
			SELECT @EmailAttachment =(EmailAttachmentRootDir + REPLACE(@ResponseText,'"','')) FROM MasterEmailType WHERE IEmailType=7

			DROP TABLE #TEMP_Batch_Data
		END
		ELSE IF @EmailType = 8 -- VPE Meeting Minutes for Internal Participants 
		BEGIN
			--Getting Meeting Minutes Details for internal participant			
			DECLARE @AgendaTable NVARCHAR(MAX)='',@TaskTable NVARCHAR(MAX)=''
			
			SET @Calendarquery =''
							
			SELECT DISTINCT
			VMH.IMeetingID
			,ISNULL(MeetingTitle,'') AS MeetingTitle
			,ISNULL(CreatedByUserName,'') AS CreatedByUserName
			,CONVERT(NVARCHAR(20),MeetingStartTime,107) AS MeetingDate
			,CONVERT(varchar(15),CAST(MeetingStartTime AS TIME),100) AS MeetingStartTime
			,CONVERT(varchar(15),CAST(MeetingEndTime AS TIME),100) AS MeetingEndTime
			,ISNULL(C.DCompanyName,'') AS CompanyName 
			,ISNULL(C.DCustGrpName,'') AS CustomerGroup
			,ISNULL(C.DivDesc,'') AS Division
			,CONVERT(NVARCHAR(10),V.VendorNumber)+' - '+ V.OrigVendName AS Supplier
			,ISNULL(HeadedByUserName,'') AS HeadedByUserName
			,ISNULL(Location,'') AS Location
			,ISNULL(VW_IP.InternalParticipantsName,'') AS 'InternalParticipantsName'
			,ISNULL(VW_EP.ExternalParticipantsName,'') AS 'ExternalParticipantsName'
			,ISNULL(VW_A.AbsenteeParticipantsName,'') AS 'AbsenteesName'
			INTO #VPEMeetingDetails
			FROM VPEMeetingHeader VMH WITH(NOLOCK)
			JOIN EGCAPPSDWHS.[dbo].[DBW-SV02-DivisionDescription] C WITH (NOLOCK)
			ON VMH.CONO = C.CONO
			AND VMH.CustGroupID = C.DCustGroupID
			AND VMH.Division = C.Div			
			JOIN EGCAPPSDWHS.dbo.[DBW-SV11-Vendor] V WITH(NOLOCK)
			ON C.CONO = V.CompanyNumber
			AND VMH.VendorNumber = V.VendorNumber			
			LEFT OUTER JOIN VW_VPEMeetingInternalParticipants VW_IP WITH(NOLOCK)
			ON VMH.IMeetingID = VW_IP.IMeetingID
			LEFT OUTER JOIN VW_VPEMeetingExternalParticipants VW_EP WITH(NOLOCK)
			ON VMH.IMeetingID = VW_EP.IMeetingID
			LEFT OUTER JOIN [VW_VPEAbsentee] VW_A WITH(NOLOCK)
			ON VMH.IMeetingID = VW_A.IMeetingID
			WHERE VMH.IMeetingID = @IDVal 
			AND ISNULL(Inactive,0) = 0

			--Getting Agendas for the Meeting
			SELECT
			CONVERT(NVARCHAR(10),ROW_NUMBER() OVER(ORDER BY VPA.IAgendaID)) + '. ' + ISNULL(AgendaTitle,'') AS AgendaTitle
			,ISNULL(VPN.Notes,'') AS Notes
			,CASE WHEN IsDone=1 THEN 'Completed' ELSE 'Remaining' END AS AgendaStatus
			INTO #VPEAgendas
			FROM VPEMeetingAgenda VPA
			LEFT OUTER JOIN [VPEMeetingAgendaNotes] VPN WITH(NOLOCK)
			ON VPA.IMeetingId = VPN.IMeetingId
			AND VPA.IAgendaID = VPN.IAgendaID
			WHERE VPA.IMeetingID = @IDVal 
			AND ISNULL(Inactive,0) = 0

			SELECT @AgendaCount = COUNT(*)
								FROM #VPEAgendas

			--Getting Tasks for the meeting
			SELECT CONVERT(NVARCHAR(10),ROW_NUMBER() OVER(ORDER BY TA.IMeetingTaskID)) +'. ' + TaskTitle AS TaskTitle
			,TaskDesc
			,CONVERT(NVARCHAR(20),StartDate,107) AS StartDate
			,CONVERT(NVARCHAR(20),DueDate,107) AS DueDate
			,ISNULL(VW_TA.TaskAssignedUsersName,'') AS TaskAssignedUsersName
			,VR.ReferenceText as [Priority]
			,VS.ReferenceText as [Status]
			INTO #VPEMeetingTasks
			FROM VPEMeetingTasks  TA
			JOIN VPEReferences VR WITH(NOLOCK)
			ON TA.ITaskPriorityID = VR.ReferenceKey
			AND VR.ReferenceTypeKey = 3
			JOIN VPEReferences VS WITH(NOLOCK)
			ON TA.ITaskStatusID = VS.ReferenceKey 
			AND VS.ReferenceTypeKey = 2
			LEFT OUTER JOIN VW_VPETaskAssignedUsers VW_TA WITH(NOLOCK)
			ON TA.IMeetingTaskID = VW_TA.IMeetingTaskID	
			WHERE ISNULL(TA.Inactive,0) = 0
			AND TA.IMeetingID IN (SELECT VMH1.IMeetingID	
			FROM VPEMeetingHeader VMH
			INNER JOIN VPEMeetingHeader VMH1
			ON VMH.IRecurrenceID = VMH1.IRecurrenceID
			WHERE VMH.IMeetingID = @IDVal)

			SELECT @TaskCount = COUNT(*)
								FROM #VPEMeetingTasks

			SELECT @Subject = 'Supplier Collaboration-Meeting Minutes–' + MeetingTitle 
							  FROM #VPEMeetingDetails N	

			SELECT DISTINCT @StrBody = '<font style="font-size:11pt;font-family:calibri;"><b><font style="font-size:14pt;">' + MeetingTitle +'</font></b><br>' 
									+ 'Created by ' + CreatedByUserName + '<br><br>'
									+ '<table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3" width="95%">'
									+ '<tr><td width="10%"><b>Meeting Date</b></td><td width="30%">' + MeetingDate + ' ' +
									+ MeetingStartTime + ' - ' + MeetingEndTime
									+ '</td><td><b>Room</b></td><td>' + Location  +'</td></tr>'
									+ '<tr><td valign="top"><b>Supplier</b></td><td>' + Supplier + '</td>'
									+ '<td><b>EGC Division</b></td><td>' + + CompanyName  +', '+ CustomerGroup  +', '+ Division
									+ '</td></tr>'
									+ '<tr><td><b>Headed By</b></td><td>' + HeadedByUserName +'</td>'
									+ '<td><b>Participants</b></td><td>' + + InternalParticipantsName + ',' + ExternalParticipantsName  + '</td></tr>'
									+ '<tr><td><b>Absentees</b></td><td colspan="3">' + AbsenteesName +'</td></tr></table><br>'
			FROM #VPEMeetingDetails
	
			SET @AgendaTable='<b><font style="font-size:13pt;font-family:calibri;">Agendas</font></b><br>' + '<table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3" width="90%"><tr><td width="20%"><b>Title</b></td><td width="30%"><b>Agenda Comments</b></td><td width="10%"><b>Status</b></td></tr>' 
			SELECT @AgendaTable +='<tr><td>' + AgendaTitle +'</td><td>' + Notes + '</td><td>' + AgendaStatus + '</td></tr>'
			FROM #VPEAgendas 

			SET @AgendaTable += '</table>'

			SET @TaskTable='<br><b><font style="font-size:13pt;font-family:calibri;">Tasks</font></b><br><br><table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3" width="100%"><tr><td width="20%"><b>Title</b></td><td width="20%"><b>Task Description</b></td><td width="20%"><b>Assigned To</b></td><td width="10%"><b>Start Date</b>'
			+'</td><td width="10%"><b>Due Date</b></td><td width="10%"><b>Priority</b></td><td width="10%"><b>Status</b></td></tr>'
			SELECT @TaskTable += '<tr><td>' + TaskTitle +'</td><td>' + TaskDesc +  '</td><td>' + TaskAssignedUsersName + '</td><td>' + StartDate + '</td><td>' + DueDate + '</td><td>' + [Priority] + '</td><td>' + [Status] + '</td></tr>'
								FROM #VPEMeetingTasks

			SET @TaskTable += '</table>'

			SET @PageURL ='/redirect.aspx?frommode=VPE&ID='

			SELECT @StrBody = @StrBody + CASE WHEN @AgendaCount > 0 THEN @AgendaTable  ELSE ''  END 
							+ CASE WHEN @TaskCount > 0 THEN @TaskTable ELSE '' END + ''
							+'<br><a href="' + @siteUrl + @PageURL + CONVERT(NVARCHAR(100),@IDVal) +'">View Meeting Minutes</a><br><br>Thanks and regards<br>' + CreatedByUserName + '</font>'
			FROM #VPEMeetingDetails	

			DROP TABLE #VPEMeetingDetails
			DROP TABLE #VPEMeetingTasks
			DROP TABLE #VPEAgendas


		END
		ELSE IF @EmailType = 9 -- VPE Meeting Minutes for Supplier Contact
		BEGIN

			set @EmailTo=REPLACE(@EmailTo ,',',';')

			--Getting Meeting Minutes Details for supplier contact		
			DECLARE @AgendaTableForSupplier NVARCHAR(MAX)='',@TaskTableForSupplier NVARCHAR(MAX)='',@IsSupplierPortalEnabled INT			
			
			SET @Calendarquery =''

			SELECT @IsSupplierPortalEnabled = ISNULL(V.SupplierPortalEnabled,0)
			FROM VPEMeetingHeader VMH WITH(NOLOCK)			
			JOIN EGCAPPSDWHS.dbo.[DBW-SV11-Vendor] V WITH(NOLOCK)
			ON VMH.CONO= V.CompanyNumber
			AND VMH.VendorNumber = V.VendorNumber
			WHERE VMH.IMeetingID = @IDVal
			AND V.CompanyNumber NOT IN (SELECT CompanyNumber FROM EGCAppsDWHS.dbo.mstrInvalidCompanies)
			AND ISNULL(Inactive,0) = 0			
							
			SELECT DISTINCT
			VMH.IMeetingID
			,ISNULL(MeetingTitle,'') AS MeetingTitle
			,ISNULL(CreatedByUserName,'') AS CreatedByUserName
			,CONVERT(NVARCHAR(20),MeetingStartTime,107) AS MeetingDate
			,CONVERT(varchar(15),CAST(MeetingStartTime AS TIME),100) AS MeetingStartTime
			,CONVERT(varchar(15),CAST(MeetingEndTime AS TIME),100) AS MeetingEndTime
			,ISNULL(C.DCompanyName,'') AS CompanyName 
			,ISNULL(C.DCustGrpName,'') AS CustomerGroup
			,ISNULL(C.DivDesc,'') AS Division
			,CONVERT(NVARCHAR(10),V.VendorNumber)+' - '+ V.OrigVendName AS Supplier
			,ISNULL(HeadedByUserName,'') AS HeadedByUserName
			,ISNULL(Location,'') AS Location
			,ISNULL(VW_IP.InternalParticipantsName,'') AS 'InternalParticipantsName'
			,ISNULL(VW_EP.ExternalParticipantsName,'') AS 'ExternalParticipantsName'
			,ISNULL(VW_A.AbsenteeParticipantsName,'') AS 'AbsenteesName'
			INTO #VPEMeetingDetailsForSupplier
			FROM VPEMeetingHeader VMH
			JOIN EGCAPPSDWHS.[dbo].[DBW-SV02-DivisionDescription] C WITH (NOLOCK)
			ON VMH.CONO = C.CONO
			AND VMH.CustGroupID = C.DCustGroupID
			AND VMH.Division = C.Div		
			JOIN EGCAPPSDWHS.dbo.[DBW-SV11-Vendor] V WITH(NOLOCK)
			ON C.CONO = V.CompanyNumber
			AND VMH.VendorNumber = V.VendorNumber		
			LEFT OUTER JOIN VW_VPEMeetingInternalParticipants VW_IP
			ON VMH.IMeetingID = VW_IP.IMeetingID
			LEFT OUTER JOIN VW_VPEMeetingExternalParticipants VW_EP
			ON VMH.IMeetingID = VW_EP.IMeetingID
			LEFT OUTER JOIN [VW_VPEAbsentee] VW_A
			ON VMH.IMeetingID = VW_A.IMeetingID
			WHERE VMH.IMeetingID = @IDVal 
			AND ISNULL(Inactive,0) = 0

			--Getting Agendas for the Meeting
			SELECT
			CONVERT(NVARCHAR(10),ROW_NUMBER() OVER(ORDER BY VPA.IAgendaID)) + '. ' + ISNULL(AgendaTitle,'') AS AgendaTitle
			,ISNULL(VPN.Notes,'') AS Notes
			,CASE WHEN IsDone=1 THEN 'Completed' ELSE 'Remaining' END AS AgendaStatus
			INTO #VPEAgendasForSupplier
			FROM VPEMeetingAgenda VPA
			LEFT OUTER JOIN [VPEMeetingAgendaNotes] VPN WITH(NOLOCK)
			ON VPA.IMeetingId = VPN.IMeetingId
			AND VPA.IAgendaID = VPN.IAgendaID
			WHERE VPA.IMeetingID = @IDVal 
			AND ISNULL(Inactive,0) = 0

			SELECT @AgendaCount = COUNT(*)
			FROM #VPEAgendasForSupplier

			--Getting Tasks for the meeting
			SELECT CONVERT(NVARCHAR(10),ROW_NUMBER() OVER(ORDER BY TA.IMeetingTaskID)) +'. ' + TaskTitle AS TaskTitle
			,TaskDesc
			,CONVERT(NVARCHAR(20),StartDate,107) AS StartDate
			,CONVERT(NVARCHAR(20),DueDate,107) AS DueDate
			,ISNULL(VW_TA.TaskAssignedUsersName,'') AS TaskAssignedUsersName
			,VR.ReferenceText as [Priority]
			,VS.ReferenceText as [Status]
			INTO #VPEMeetingTasksForSupplier
			FROM VPEMeetingTasks  TA
			JOIN VPEReferences VR WITH(NOLOCK)
			ON TA.ITaskPriorityID = VR.ReferenceKey
			AND VR.ReferenceTypeKey = 3
			JOIN VPEReferences VS WITH(NOLOCK)
			ON TA.ITaskStatusID = VS.ReferenceKey 
			AND VS.ReferenceTypeKey = 2
			LEFT OUTER JOIN VW_VPETaskAssignedUsers VW_TA WITH(NOLOCK)
			ON TA.IMeetingTaskID = VW_TA.IMeetingTaskID	
			WHERE ISNULL(TA.Inactive,0) = 0
			AND TA.IMeetingID IN (SELECT VMH1.IMeetingID	
			FROM VPEMeetingHeader VMH
			INNER JOIN VPEMeetingHeader VMH1
			ON VMH.IRecurrenceID = VMH1.IRecurrenceID
			WHERE VMH.IMeetingID = @IDVal)	
			
			SELECT @TaskCount = COUNT(*)
			FROM #VPEMeetingTasksForSupplier		
			
			IF @IsSupplierPortalEnabled = 1 
				BEGIN

					SELECT @Subject = 'Supplier Collaboration-Meeting Minutes–' + MeetingTitle 
							  FROM #VPEMeetingDetailsForSupplier N	

					SELECT DISTINCT @StrBody = '<font style="font-size:11pt;font-family:calibri;"><b><font style="font-size:14pt;">' + MeetingTitle +'</font></b><br>' 
												+ 'Created by ' + CreatedByUserName + '<br><br>'
												+ '<table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3" width="95%">'
												+ '<tr><td width="10%"><b>Meeting Date</b></td><td width="30%">' + MeetingDate + ' ' +
												+ MeetingStartTime + ' - ' + MeetingEndTime
												+ '</td><td><b>Room</b></td><td>' + Location  +'</td></tr>'
												+ '<tr><td valign="top"><b>Supplier</b></td><td>' + Supplier + '</td>'
												+ '<td><b>EGC Division</b></td><td>' + + CompanyName  +', '+ CustomerGroup  +', '+ Division
												+ '</td></tr>'
												+ '<tr><td><b>Headed By</b></td><td>' + HeadedByUserName +'</td>'
												+ '<td><b>Participants</b></td><td>' + + InternalParticipantsName + ',' + ExternalParticipantsName  + '</td></tr>'
												+ '<tr><td><b>Absentees</b></td><td colspan="3">' + AbsenteesName +'</td></tr></table><br>'
					FROM #VPEMeetingDetailsForSupplier
	
					SET @AgendaTableForSupplier='<b><font style="font-size:13pt;font-family:calibri;">Agendas</font></b><br>' + '<table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3" width="90%"><tr><td width="20%"><b>Title</b></td><td width="30%"><b>Agenda Comments</b></td><td width="10%"><b>Status</b></td></tr>' 
					SELECT @AgendaTableForSupplier += '<tr><td>' + AgendaTitle +'</td><td>' + Notes + '</td><td>' + AgendaStatus + '</td></tr>'
					FROM #VPEAgendasForSupplier 

					SET @AgendaTableForSupplier +='</table>'

					SET @TaskTableForSupplier ='<br><b><font style="font-size:13pt;font-family:calibri;">Tasks</font></b><br><table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3" width="100%"><tr><td width="20%"><b>Title</b></td><td width="20%"><b>Task Description</b></td><td width="20%"><b>Assigned To</b></td><td width="10%"><b>
					Start Date</b></td><td width="10%"><b>Due Date</b></td><td width="10%"><b>Priority</b></td><td width="10%"><b>Status</b></td></tr>'
					
					SELECT @TaskTableForSupplier += '<tr><td>' + TaskTitle +'</td><td>' + TaskDesc +  '</td><td>' + TaskAssignedUsersName + '</td><td>' + StartDate + '</td><td>' + DueDate + '</td><td>' + [Priority] + '</td><td>' + [Status] + '</td></tr>'
												FROM #VPEMeetingTasksForSupplier

					SET @TaskTableForSupplier +='</table>'

					SELECT @StrBody = @StrBody +  CASE WHEN @AgendaCount > 0 THEN @AgendaTableForSupplier ELSE ''  END 
										+ CASE WHEN @TaskCount > 0 THEN @TaskTableForSupplier ELSE '' END
										+ '<br><br>'
										+'Thanks and regards<br>' + CreatedByUserName +'</font>'
										FROM #VPEMeetingDetailsForSupplier
				END
			ELSE
				BEGIN

					SELECT @Subject = 'Supplier Collaboration-Meeting Minutes–' + MeetingTitle 
									FROM #VPEMeetingDetailsForSupplier N	

					SELECT DISTINCT @StrBody = '<font style="font-size:11pt;font-family:calibri;"><b><font style="font-size:14pt;">' + MeetingTitle +'</font></b><br>' + 'Created by ' + CreatedByUserName + '<br><br>'
												+ '<table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3" width="95%">'
												+ '<tr><td width="10%"><b>Meeting Date</b></td><td width="30%">' + MeetingDate + ' ' +
												+ MeetingStartTime + ' - ' + MeetingEndTime
												+ '</td><td><b>Room</b></td><td>' + Location  +'</td></tr>'
												+ '<tr><td valign="top"><b>Supplier</b></td><td>' + Supplier + '</td>'
												+ '<td><b>EGC Division</b></td><td>' + + CompanyName  +', '+ CustomerGroup  +', '+ Division
												+ '</td></tr>'
												+ '<tr><td><b>Headed By</b></td><td>' + HeadedByUserName +'</td>'
												+ '<td><b>Participants</b></td><td>' + + InternalParticipantsName + ',' + ExternalParticipantsName  + '</td></tr>'
												+ '<tr><td><b>Absentees</b></td><td colspan="3">' + AbsenteesName +'</td></tr></table><br>'
											FROM #VPEMeetingDetailsForSupplier
	
					SET @AgendaTableForSupplier='<b><font style="font-size:13pt;font-family:calibri;">Agendas</font></b><br>' + '<table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3" width="90%"><tr><td width="20%"><b>Title</b></td><td width="30%"><b>Agenda Comments</b></td><td width="10%"><b>Status</b></td></tr>' 
					SELECT @AgendaTableForSupplier += '<tr><td>' + AgendaTitle +'</td><td>' + Notes + '</td><td>' + AgendaStatus + '</td></tr>'
					FROM #VPEAgendasForSupplier 

					SET @AgendaTableForSupplier +='</table>'

					SET @TaskTableForSupplier ='<br><b><font style="font-size:13pt;font-family:calibri;">Tasks</font></b><br><table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3" width="100%"><tr><td width="20%"><b>Title</b></td><td width="20%"><b>Task Description</b></td><td width="20%"><b>Assigned To</b></td><td width="10%"><b>
					Start Date</b></td><td width="10%"><b>Due Date</b></td><td width="10%"><b>Priority</b></td><td width="10%"><b>Status</b></td></tr>'
					
					SELECT @TaskTableForSupplier += '<tr><td>' + TaskTitle +'</td><td>' + TaskDesc +  '</td><td>' + TaskAssignedUsersName + '</td><td>' + StartDate + '</td><td>' + DueDate + '</td><td>' + [Priority] + '</td><td>' + [Status] + '</td></tr>'
												FROM #VPEMeetingTasksForSupplier

					SET @TaskTableForSupplier +='</table>'

					SELECT @StrBody = @StrBody +  CASE WHEN @AgendaCount > 0 THEN @AgendaTableForSupplier ELSE ''  END 
										+ CASE WHEN @TaskCount > 0 THEN @TaskTableForSupplier ELSE '' END
										+ '<br><br>'
										+'Thanks and regards<br>' + CreatedByUserName +'</font>'
										FROM #VPEMeetingDetailsForSupplier
				END				

			DROP TABLE #VPEMeetingDetailsForSupplier
			DROP TABLE #VPEMeetingTasksForSupplier
			DROP TABLE #VPEAgendasForSupplier
  
		END
		ELSE IF @EmailType = 10 -- VPE Weekly Open Task Report
		BEGIN
			SET @StrBody = @EmailBody
			SET @EmailFrom ='noreply@theegc.com'
			SET @ReplyTo = NULL
			SET @Calendarquery =''
		END
		ELSE IF @EmailType = 11 -- VPE Calendar invite to all participants
		BEGIN

			SELECT @SupplierPortalEnabled = ISNULL(V.SupplierPortalEnabled,0)
			FROM VPEMeetingHeader VMH WITH(NOLOCK)			
			JOIN EGCAPPSDWHS.dbo.[DBW-SV11-Vendor] V WITH(NOLOCK)
			ON VMH.CONO= V.CompanyNumber
			AND VMH.VendorNumber = V.VendorNumber
			WHERE VMH.IMeetingID = @IDVal
			AND V.CompanyNumber NOT IN (SELECT CompanyNumber FROM EGCAppsDWHS.dbo.mstrInvalidCompanies)
			AND ISNULL(Inactive,0) = 0	

			--Insert into Supplier Portal Email log if supplier is supplier portal enabled.
			IF @SupplierPortalEnabled = 1 AND @RecipientType = 'E' BEGIN
			
				INSERT INTO [PSMI-SQLSTAGE\EXTAPPS].Supplierportal.dbo.EmailNotificationLog(IdVal,EmailType,EmailTo,LogOnDatettime)
				SELECT @IDVal,4,@EmailTo,GETDATE()

				UPDATE EmailNotificationLog
				SET IsProcessed = 1
				WHERE IEmailSrNo=@SrNo

				--Update EmailTo to blank to not process this email
				SET @EmailTo =''

			END
			ELSE BEGIN
				--sending calendar event to participants
				IF @RecipientType = 'I'
					BEGIN
						SELECT @IParticipantID = ISNULL(IInternalParticipantID,'')
						FROM [VPEInternalParticipant]
						WHERE IMeetingID = @IDVal
						AND LTRIM(RTRIM(ParticipantEmailID)) = LTRIM(RTRIM(@EmailTo))
					END
				ELSE
					BEGIN
						SELECT @IParticipantID =  ISNULL(IExternalParticipantID,'')
						FROM [VPEExternalParticipant]
						WHERE IMeetingID = @IDVal
						AND LTRIM(RTRIM(ParticipantEmailID)) = LTRIM(RTRIM(@EmailTo))
				END

				--Redirecting users to VPE's Landing Auth page that doent required any authentication

				SET @PageURL ='/LandingAuth.aspx?frommode=VPEAvailability&ID=' + CONVERT(NVARCHAR(100),@IDVal) 
							 + '&IParticipantID=' + CONVERT(NVARCHAR(100),@IParticipantID) + '&ParticipantType=' + @RecipientType	
							 
				DECLARE @strPgUrl NVARCHAR(MAX)= 'To confirm your participation, please click on the link below. If you would like to inform any change or non-participation please reply to this email<br>'
										+ '<a href="' + @VPESiteUrl + @PageURL +'">Please click here to confirm your availability</a><br>'
						
				SELECT DISTINCT @StrBody = '<font style="font-size:11pt;font-family:calibri;">' +  
										+ '<br>Hi,<br><br>You are invited for a meeting. The details are given below. The meeting invite is attached with this email,' 
										+ 'please open the attachment and save it to show it in your calendar.<br><br><font >Meeting Subject - ' + MeetingTitle
										+'</font><br><br>' + '<table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3" width="95%">'
										+ '<tr><td width="10%"><b>Meeting Date</b></td><td width="30%">' + CONVERT(NVARCHAR(20),MeetingStartTime,107)  + ' ' +
				 
										+ CONVERT(varchar(15),CAST(MeetingStartTime AS TIME),100)	
										+ ' - ' + CONVERT(varchar(15),CAST(MeetingEndTime AS TIME),100) 
										+ '</td><td><b>Room</b></td><td>' + Location  +'</td></tr>'
										+ '<tr><td valign="top"><b>Supplier</b></td><td>' + CONVERT(NVARCHAR(10),V.VendorNumber)+' - '+ V.OrigVendName + '</td>'
										+ '<td><b>EGC Division</b></td><td>' + ISNULL(C.DCompanyName,'')  +', '+ ISNULL(C.DCustGrpName,'')  +', '+ ISNULL(C.DivDesc,'') 
										+ '</td></tr>'
										+ '<tr><td><b>Headed By</b></td><td>' + ISNULL(HeadedByUserName,'') +'</td>'
										+'<td><b>Participants</b></td><td>' + ISNULL(VW_IP.InternalParticipantsName,'') + ',' + ISNULL(VW_EP.ExternalParticipantsName,'') + '</td></tr>'
										+ '</table><br>' +
										CASE WHEN ISNULL(@IParticipantID,'') = '' THEN '' ELSE @strPgUrl END -- Sending mail to Headed by and not creating acceptance link 
										+ '<br>Thanks and regards<br>' + CreatedByUserName + '</font>',
								@EmailFrom =CreatedByEmailID,@ReplyTo=CreatedByEmailID,
								@EmailAttachment =	STUFF((SELECT DISTINCT ';' + ISNULL(VD.FilePath,'')  
													FROM VPEDivisionDocuments VD WITH(NOLOCK)											
													WHERE VD.CONO = VMH.CONO
													AND VD.Division =  VMH.Division FOR XML PATH ('')), 1, 1, '')																		
				FROM VPEMeetingHeader VMH
				JOIN EGCAPPSDWHS.[dbo].[DBW-SV02-DivisionDescription] C WITH (NOLOCK)
				ON VMH.CONO = C.CONO
				AND VMH.CustGroupID = C.DCustGroupID
				AND VMH.Division = C.Div		
				JOIN EGCAPPSDWHS.dbo.[DBW-SV11-Vendor] V WITH(NOLOCK)
				ON C.CONO = V.CompanyNumber
				AND VMH.VendorNumber = V.VendorNumber		
				LEFT OUTER JOIN VW_VPEMeetingInternalParticipants VW_IP
				ON VMH.IMeetingID = VW_IP.IMeetingID
				LEFT OUTER JOIN VW_VPEMeetingExternalParticipants VW_EP
				ON VMH.IMeetingID = VW_EP.IMeetingID 			
				WHERE VMH.IMeetingID = @IDVal
				AND ISNULL(Inactive,0) = 0		
				
				SELECT @Calendarquery ='
									--
									SET NOCOUNT ON;
									--
									SELECT ''BEGIN:VCALENDAR''
									+ CHAR(13)
									+ ''PRODID:-//My Company//Company Calendar//EN''
									+ CHAR(13)
									+ ''VERSION:2.0''
									+ CHAR(13)
									+ ''X-WR-TIMEZONE:Europe/Dublin''
									+ CHAR(13)
									+ ''BEGIN:VTIMEZONE''
									+ CHAR(13)
									+ ''TZID:America/New_York''
									+ CHAR(13)
									+ ''BEGIN:DAYLIGHT''
									+ CHAR(13)
									+ ''TZOFFSETFROM:-0500''
									+ CHAR(13)
									+ ''TZOFFSETTO:-0400''
									+ CHAR(13)
									+ ''DTSTART:' + CONVERT(VARCHAR,dbo.[convertDateToLocal] (MeetingStartTime,GMTTimeDifference,0),126) + '''
									+ CHAR(13)
									+ ''TZNAME:GMT''
									+ CHAR(13)
									+ ''END:DAYLIGHT''
									+ CHAR(13)
									+ ''END:VTIMEZONE''
									+ CHAR(13)
									+ ''METHOD:PUBLISH''
									+ CHAR(13)
									+ ''BEGIN:VEVENT''
									+ CHAR(13)
									+ ''CLASS:PUBLIC''
									+ CHAR(13)
									+ ''DESCRIPTION;FMTTYPE=text/html:<!doctype html><html><body>'+@StrBody+'</body></html>'' 
									+ CHAR(13)
									+ ''X-ALT-DESC;FMTTYPE=text/html:<!doctype html><html><body>'+@StrBody+'</body></html>''
									+ CHAR(13)
									+ ''SUMMARY:'+ MeetingTitle  + '''
									+ CHAR(13)
									+ ''UID:EGCMeetingID' + CONVERT(VARCHAR,IMeetingID) +'''
									+ CHAR(13)
									+ ''ORGANIZER;CN=' + HeadedByUserName +':mailto:' + HeadedByEmailID +'''
									+ CHAR(13)
									+ ''DTEND;TZID=Europe/Dublin:' + CONVERT(VARCHAR,dbo.[convertDateToLocal] (MeetingEndTime,GMTTimeDifference,0),126) + '''
									+ CHAR(13)
									+ ''DTSTART;TZID=Europe/Dublin:' + CONVERT(VARCHAR,dbo.[convertDateToLocal] (MeetingStartTime,GMTTimeDifference,0),126) + '''
									+ CHAR(13)
									+ ''LOCATION:'+ISNULL(C.DCompanyName,'')  +'>> '+ ISNULL(C.DCustGrpName,'')  +'>> '+ ISNULL(C.DivDesc,'') +'>> '+ Location + '''
									+ CHAR(13)
									+ %ATTENDEE%
									+ ''END:VEVENT''
									+ CHAR(13)
									+ ''END:VCALENDAR''
									'
				FROM VPEMeetingHeader VMH WITH(NOLOCK)
				JOIN EGCAPPSDWHS.[dbo].[DBW-SV02-DivisionDescription] C WITH(NOLOCK)
				ON VMH.CONO = C.CONO
				AND VMH.CustGroupID = C.DCustGroupID
				AND VMH.Division = C.Div
				WHERE IMeetingID = @IDVal

				SET @internalAttendee =''
				SET @externalAttendee =''
				SELECT @internalAttendee +='''ATTENDEE;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION;RSVP=TRUE:MAILTO:' +  ParticipantEmailID +'''
				+ CHAR(13) +' FROM [VPEInternalParticipant] NOLOCK WHERE imeetingID=@IDVal

				SELECT @externalAttendee +='''ATTENDEE;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION;RSVP=TRUE:MAILTO:' +  ParticipantEmailID +'''
				+ CHAR(13) +' FROM [VPEExternalParticipant] NOLOCK WHERE imeetingID=@IDVal

				SET @Calendarquery = REPLACE(@Calendarquery,'%ATTENDEE%', @internalAttendee + @externalAttendee)

				SET @EmailCC = ''
				SET @EmailBCC= ''
				SET @Subject = 'Supplier Collaboration-' + @Subject
			END
		END
		ELSE IF @EmailType = 12 -- VPE Daily Status to user for open tasks
		BEGIN
			SET @StrBody = @EmailBody
			SET @EmailFrom ='noreply@theegc.com'
			SET @ReplyTo ='noreply@theegc.com'
			SET @Calendarquery =''
		END
		ELSE IF @EmailType = 13 -- VPE Cancelled meeting
		BEGIN
			--sending calendar event to participants
			SELECT @SupplierPortalEnabled = ISNULL(V.SupplierPortalEnabled,0)
			FROM VPEMeetingHeader VMH WITH(NOLOCK)			
			JOIN EGCAPPSDWHS.dbo.[DBW-SV11-Vendor] V WITH(NOLOCK)
			ON VMH.CONO= V.CompanyNumber
			AND VMH.VendorNumber = V.VendorNumber
			WHERE VMH.IMeetingID = @IDVal
			AND V.CompanyNumber NOT IN (SELECT CompanyNumber FROM EGCAppsDWHS.dbo.mstrInvalidCompanies)
			AND ISNULL(Inactive,0) = 0	

			SET @EmailTo = REPLACE(@EmailTo,',',';')
			--Insert into Supplier Portal Email log if supplier is supplier portal enabled.
			IF @SupplierPortalEnabled = 1 BEGIN
			
				INSERT INTO [PSMI-SQLSTAGE\EXTAPPS].Supplierportal.dbo.EmailNotificationLog(IdVal,EmailType,EmailTo,LogOnDatettime)
				SELECT @IDVal,5,ParticipantEmailID,GETDATE()
				FROM [VPEExternalParticipant] NOLOCK 
				WHERE imeetingID=@IDVal
				
			END

			SELECT DISTINCT @EmailTo =  STUFF((
											SELECT ';' + u.ParticipantEmailID
											FROM VPEInternalParticipant u
											WHERE IMeetingID=@IDVal
											ORDER BY u.ParticipantUserName
											FOR XML PATH('')
											),1,1,'') 

			SELECT DISTINCT @StrBody = '<font style="font-size:11pt;font-family:calibri;">' 
			+ '<br>Meeting scheduled on ' + FORMAT(MeetingStartTime, 'ddd') + ',  ' + convert(nvarchar(20),MeetingStartTime,0) 	+
			
			' - ' + CONVERT(varchar(15),CAST(MeetingEndTime AS TIME),100) + ' has been cancelled.<br>please open the attachment and click on "Remove from Calendar" to remove meeting from your calendar.</font>'
			,@EmailFrom =CreatedByEmailID,@ReplyTo=CreatedByEmailID
			FROM VPEMeetingHeader
			WHERE IMeetingID = @IDVal
				
			SELECT @Calendarquery ='
								--
								SET NOCOUNT ON;
								--
								SELECT ''BEGIN:VCALENDAR''
								+ CHAR(13)
								+ ''PRODID:-//My Company//Company Calendar//EN''
								+ CHAR(13)
								+ ''VERSION:2.0''
								+ CHAR(13)
								+ ''X-WR-TIMEZONE:Europe/Dublin''
								+ CHAR(13)
								+ ''BEGIN:VTIMEZONE''
								+ CHAR(13)
								+ ''TZID:America/New_York''
								+ CHAR(13)
								+ ''BEGIN:DAYLIGHT''
								+ CHAR(13)
								+ ''TZOFFSETFROM:-0500''
								+ CHAR(13)
								+ ''TZOFFSETTO:-0400''
								+ CHAR(13)
								+ ''DTSTART:' + CONVERT(VARCHAR,dbo.[convertDateToLocal] (MeetingStartTime,GMTTimeDifference,0),126) + '''
								+ CHAR(13)
								+ ''TZNAME:GMT''
								+ CHAR(13)
								+ ''END:DAYLIGHT''
								+ CHAR(13)
								+ ''END:VTIMEZONE''
								+ CHAR(13)
								+ ''METHOD:CANCEL''
								+ CHAR(13)
								+ ''BEGIN:VEVENT''
								+ CHAR(13)
								+ ''CLASS:PUBLIC''
								+ CHAR(13)
								+ ''DESCRIPTION;FMTTYPE=text/html:<!doctype html><html><body>'+@StrBody+'</body></html>'' 
								+ CHAR(13)
								+ ''X-ALT-DESC;FMTTYPE=text/html:<!doctype html><html><body>'+@StrBody+'</body></html>''
								+ CHAR(13)
								+ ''SUMMARY:'+ MeetingTitle + '''
								+ CHAR(13)
								+''UID:EGCMeetingID' + CONVERT(VARCHAR,IMeetingID) +'''
								+ CHAR(13)
								+ ''ORGANIZER;CN=' + HeadedByUserName +':mailto:' + HeadedByEmailID +'''
								+ CHAR(13)
								+ ''DTEND;TZID=Europe/Dublin:' + CONVERT(VARCHAR,dbo.[convertDateToLocal] (MeetingEndTime,GMTTimeDifference,0), 126) + '''
								+ CHAR(13)
								+ ''DTSTART;TZID=Europe/Dublin:' + CONVERT(VARCHAR,dbo.[convertDateToLocal] (MeetingStartTime,GMTTimeDifference,0), 126) + '''
								+ CHAR(13)
								+ ''LOCATION:'+ISNULL(C.DCompanyName,'')  +'>> '+ ISNULL(C.DCustGrpName,'')  +'>> '+ ISNULL(C.DivDesc,'') +'>> '+ Location + '''
								+ CHAR(13)
								+ %ATTENDEE%
								+ ''END:VEVENT''
								+ CHAR(13)
								+ ''END:VCALENDAR''
								'
			FROM VPEMeetingHeader VMH WITH(NOLOCK)
			JOIN EGCAPPSDWHS.[dbo].[DBW-SV02-DivisionDescription] C WITH(NOLOCK)
			ON VMH.CONO = C.CONO
			AND VMH.CustGroupID = C.DCustGroupID
			AND VMH.Division = C.Div
			WHERE IMeetingID = @IDVal

			SET @internalAttendee =''
			SET @externalAttendee =''

			SELECT @internalAttendee +='''ATTENDEE;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION:MAILTO:' +  ParticipantEmailID +'''
			+ CHAR(13) +' FROM [VPEInternalParticipant] NOLOCK WHERE imeetingID=@IDVal

			SELECT @externalAttendee +='''ATTENDEE;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION:MAILTO:' +  ParticipantEmailID +'''
			+ CHAR(13) +' FROM [VPEExternalParticipant] NOLOCK WHERE imeetingID=@IDVal

			SET @Calendarquery = REPLACE(@Calendarquery,'%ATTENDEE%', @internalAttendee + @externalAttendee)

			SET @Subject = 'Supplier Collaboration-' + @Subject 
		

		END
		
		ELSE IF @EmailType = 14 -- VPE Meeting Participant Availability
		BEGIN 
			IF @RecipientType = 'I'
				BEGIN
					SELECT @ParticipantEmailID = ParticipantEmailID,@ParticipantUserName =ParticipantUserName
					FROM [VPEInternalParticipant]
					WHERE IInternalParticipantID = @IParticipantID
				END
			ELSE
				BEGIN
					SELECT @ParticipantEmailID = ParticipantEmailID,@ParticipantUserName =ParticpantsName
					FROM [VPEExternalParticipant]
					WHERE IExternalParticipantID = @IParticipantID
				END
			
			--Get all participants list along with their availability status
			SELECT  ParticipantUserName AS Participant,'I' AS ParticipantType,
			CASE ISNULL(RT.ReferenceKey,0) WHEN 0 THEN 'Notify' ELSE RT.ReferenceText END AS [Availability],
			CASE WHEN ISNULL(RT.ReferenceKey,0) = 3 
			THEN FORMAT(DATEADD(MINUTE,GMTTimeDifference, ProposedStartTime), 'ddd') + '-' + convert(nvarchar(20),DATEADD(MINUTE,GMTTimeDifference, ProposedStartTime),0) + ' (GMT)'
			ELSE '' END AS ProposedTime
			INTO #MeetingAvailability
			FROM [VPEInternalParticipant] I
			LEFT OUTER JOIN [VPEParticipantAvailability] PA
			ON I.IMeetingID = PA.IMeetingID
			AND PA.ParticipantType = 'I'
			AND I.IInternalParticipantID = PA.IParticipantID
			LEFT OUTER JOIN VPEReferences RT WITH(NOLOCK)
			ON RT.ReferenceKey = PA.IAvailabilityStatusID
			AND ReferenceTypeKey = 5
			WHERE I.IMeetingID = @IDVal			
			UNION			
			SELECT ParticpantsName AS Participant,'E' AS ParticipantType,
			CASE ISNULL(RT.ReferenceKey,0) WHEN 0 THEN 'Notify' ELSE RT.ReferenceText END AS [Availability],
			CASE WHEN ISNULL(RT.ReferenceKey,0) = 3 
			THEN FORMAT(DATEADD(MINUTE,GMTTimeDifference, ProposedStartTime), 'ddd') + '-' + convert(nvarchar(20),DATEADD(MINUTE,GMTTimeDifference, ProposedStartTime),0) + ' (GMT)'
			ELSE '' END AS ProposedTime
			FROM [VPEExternalParticipant] E
			LEFT OUTER JOIN [VPEParticipantAvailability] PA
			ON E.IMeetingID = PA.IMeetingID
			AND PA.ParticipantType = 'E'
			AND E.IExternalParticipantID = PA.IParticipantID
			LEFT OUTER JOIN VPEReferences RT WITH(NOLOCK)
			ON RT.ReferenceKey = PA.IAvailabilityStatusID
			AND ReferenceTypeKey = 5
			WHERE E.IMeetingID = @IDVal			

			SET @ParticipantAvailabilityTable = '<table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3" width="90%">'
												+'<tr><td width="35%"><b>Participant</b></td><td width="30%"><b>Status</b></td><td width="35%"><b>Proposed Time</b></td></tr>'

			SELECT @ParticipantAvailabilityTable += '<tr><td>' + Participant + ' (' + ParticipantType + ')' + '</td><td>' + [Availability] + '</td><td>' + ProposedTime + '</td></tr>'
			FROM #MeetingAvailability

			SET @ParticipantAvailabilityTable += '</table>'
			
			DECLARE @IsrecursiveMeeting AS INT
			DECLARE @RecursiveMeetingID AS INT
			DECLARE @recinfo1 AS NVARCHAR(MAX)
			DECLARE @recmsg1 AS NVARCHAR(MAX)

			SELECT DISTINCT @IsrecursiveMeeting=CAST(ISNULL(IsRecursive,0) AS INT), @RecursiveMeetingID=ISNULL(IRecurrenceID,0),
			@recinfo1=RecuringInfo,@Subject = 'Supplier Collaboration-' + CASE WHEN IAvailabilityStatusID = 1 THEN 'Accepted: ' + MeetingTitle 
										WHEN IAvailabilityStatusID = 2 THEN 'Declined: ' + MeetingTitle 
										ELSE 'New Time Proposed: ' + MeetingTitle  END,
							@HeadedByEmailID = HeadedByEmailID,@CreatedByEmailID =CreatedByEmailID
							,@StrBody = '<font style="font-size:11pt;font-family:calibri;"><br>' + 'Hi ' + CreatedByUserName + ',<br><br>'
							+ @ParticipantUserName +' has ' + CASE WHEN IAvailabilityStatusID=1 THEN 'accepted meeting invite' WHEN IAvailabilityStatusID=2 THEN 'rejected meeting invite' ELSE 'proposed new time' END +' for "' + MeetingTitle + '", scheduled on '
							+ FORMAT(MeetingStartTime, 'ddd') + ',  ' + convert(nvarchar(20),MeetingStartTime,0)  +' - ' 
							+ CONVERT(varchar(15),CAST(MeetingEndTime AS TIME),100)  + 'recmsgcalulatedtemp <br><br>'
							+ CASE WHEN ISNULL(VA.ProposedStartTime,'') <> '' THEN 'Proposed Time:<br> ' + FORMAT(DATEADD(MINUTE,VA.GMTTimeDifference, VA.ProposedStartTime), 'ddd') + '-' + convert(nvarchar(20),DATEADD(MINUTE,VA.GMTTimeDifference, VA.ProposedStartTime),0) + '(GMT)<br><br>' ELSE '' END 
							+ CASE WHEN ISNULL(VA.Comments,'') <> '' THEN 'Comments:<br> ' + VA.Comments + '<br><br>' ELSE '' END + 'Response from all participants: <br>'
							+ @ParticipantAvailabilityTable + '<br>'
							+ '<br>Thanks and regards<br>' + @ParticipantUserName + '</font>'
							FROM VPEMeetingHeader VH
							JOIN VPEparticipantAvailability VA
							ON VH.IMeetingID = VA.IMeetingID
							WHERE VH.IMeetingID = @IDVal
							AND VA.IParticipantID = @IParticipantID			

			SET @EmailFrom =@ParticipantEmailID
			SET @replyto = @ParticipantEmailID
			SET @Calendarquery =''
			SET @EmailCC = @HeadedByEmailID
			SET @EmailTo = @CreatedByEmailID
			
			IF @IsrecursiveMeeting=1
			BEGIN

			IF @IDVal=@RecursiveMeetingID
			BEGIN

			SELECT @recmsg1=VALUE FROM OPENJSON(@recinfo1) ref WHERE ref.[key]='message'
			SELECT @StrBody=REPLACE(@StrBody,'recmsgcalulatedtemp',' ('+@recmsg1+' )')

			
			--SEND EMAIL

			PRINT 'SEND EMAIL'
			END
			ELSE
			BEGIN

			SET @EmailTo = '' 
			SET @StrBody=''
			--DONT SEND EMAIL
			END
			END
			SELECT @StrBody=REPLACE(@StrBody,'recmsgcalulatedtemp','')

			DROP TABLE #MeetingAvailability		

		END
		ELSE IF @EmailType = 15 -- NC Supplier Mail For Supplier Portal disabled suppliers
		BEGIN
			SET @Subject = 'Nonconformance ' + @Subject
			SET @StrBody = @EmailBody
		END
		ELSE IF @EmailType = 16 -- EIM Calendar invite to all participants 
		BEGIN
			--sending calendar event to participants
			 
			SELECT @IParticipantID = IInternalParticipantID
			FROM [VPEInternalParticipant]
			WHERE IMeetingID = @IDVal
			AND LTRIM(RTRIM(ParticipantEmailID)) = LTRIM(RTRIM(@EmailTo)) 

			--Redirecting users to EIM's Landing Auth page that doesnt required any authentication

			SET @PageURL ='/LandingAuth.aspx?frommode=VPEAvailability&ID=' + CONVERT(NVARCHAR(100),@IDVal) 
						 + '&IParticipantID=' + CONVERT(NVARCHAR(100),@IParticipantID) + '&ParticipantType=' + @RecipientType + '&IM=Y'			
						
			SELECT DISTINCT @StrBody = '<font style="font-size:11pt;font-family:calibri;">'  
									+ '<br>Hi,<br><br>You are invited for a meeting. The details are given below. The meeting invite is attached with this email,' 
									+ 'please open the attachment and save it to show it in your calendar.<br><br><font >Meeting Subject - ' + MeetingTitle
									+'</font><br><br>' + '<table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3" width="95%">'
									+ '<tr><td width="10%"><b>Meeting Date</b></td><td width="30%">' + CONVERT(NVARCHAR(20),MeetingStartTime,107)  + ' ' +
				 
									+ CONVERT(varchar(15),CAST(MeetingStartTime AS TIME),100)	
									+ ' - ' + CONVERT(varchar(15),CAST(MeetingEndTime AS TIME),100)
									+ '</td><td><b>Room</b></td><td>' + Location  +'</td></tr>'
									+ '<tr><td><b>EGC Division</b></td><td>' + ISNULL(C.DCompanyName,'')  +', '+ ISNULL(C.DCustGrpName,'')  +', '+ ISNULL(C.DivDesc,'') 
									+ '</td><td><b>Headed By</b></td><td>' + ISNULL(HeadedByUserName,'') +'</td></tr>'
									+ '<tr><td><b>Participants</b></td><td colspan="3">' + ISNULL(VW_IP.InternalParticipantsName,'') + '</td></tr>'
									+ '</table><br>'
									+ 'To confirm your participation, please click on the link below. If you would like to inform any change or non-participation please reply to this email<br>'
									+ '<a href="' + @VPESiteUrl + @PageURL +'">Please click here to confirm your availability</a><br>'
									+ '<br>Thanks and regards<br>' + CreatedByUserName + '</font>',
								@EmailFrom =CreatedByEmailID,@replyto=CreatedByEmailID,
								@EmailAttachment =	STUFF((SELECT DISTINCT ';' + ISNULL(VD.FilePath,'')  
												FROM VPEDivisionDocuments VD WITH(NOLOCK)											
												WHERE VD.CONO = VMH.CONO
												AND VD.Division =  VMH.Division FOR XML PATH ('')), 1, 1, '')																		
			FROM VPEMeetingHeader VMH
			JOIN EGCAPPSDWHS.[dbo].[DBW-SV02-DivisionDescription] C WITH (NOLOCK)
			ON VMH.CONO = C.CONO
			AND VMH.CustGroupID = C.DCustGroupID
			AND VMH.Division = C.Div		
			LEFT OUTER JOIN EGCAPPSDWHS.dbo.[DBW-SV11-Vendor] V WITH(NOLOCK)
			ON C.CONO = V.CompanyNumber
			AND VMH.VendorNumber = V.VendorNumber		
			LEFT OUTER JOIN VW_VPEMeetingInternalParticipants VW_IP
			ON VMH.IMeetingID = VW_IP.IMeetingID	
			WHERE VMH.IMeetingID = @IDVal
			AND ISNULL(Inactive,0) = 0		
				
			SELECT @Calendarquery ='
								--
								SET NOCOUNT ON;
								--
								SELECT ''BEGIN:VCALENDAR''
								+ CHAR(13)
								+ ''PRODID:-//My Company//Company Calendar//EN''
								+ CHAR(13)
								+ ''VERSION:2.0''
								+ CHAR(13)
								+ ''X-WR-TIMEZONE:Europe/Dublin''
								+ CHAR(13)
								+ ''BEGIN:VTIMEZONE''
								+ CHAR(13)
								+ ''TZID:America/New_York''
								+ CHAR(13)
								+ ''BEGIN:DAYLIGHT''
								+ CHAR(13)
								+ ''TZOFFSETFROM:-0500''
								+ CHAR(13)
								+ ''TZOFFSETTO:-0400''
								+ CHAR(13)
								+ ''DTSTART:' + CONVERT(VARCHAR,dbo.[convertDateToLocal] (MeetingStartTime,GMTTimeDifference,0),126) + '''
								+ CHAR(13)
								+ ''TZNAME:GMT''
								+ CHAR(13)
								+ ''END:DAYLIGHT''
								+ CHAR(13)
								+ ''END:VTIMEZONE''
								+ CHAR(13)
								+ ''METHOD:PUBLISH''
								+ CHAR(13)
								+ ''BEGIN:VEVENT''
								+ CHAR(13)
								+ ''CLASS:PUBLIC''
								+ CHAR(13)
								+ ''DESCRIPTION;FMTTYPE=text/html:<!doctype html><html><body>'+@StrBody+'</body></html>'' 
								+ CHAR(13)
								+ ''X-ALT-DESC;FMTTYPE=text/html:<!doctype html><html><body>'+@StrBody+'</body></html>''
								+ CHAR(13)
								+ ''SUMMARY:'+ MeetingTitle + '''
								+ CHAR(13)
								+ ''UID:EGCMeetingID' + CONVERT(VARCHAR,IMeetingID) +'''
								+ CHAR(13)
								+ ''ORGANIZER;CN=' + HeadedByUserName +':mailto:' + HeadedByEmailID +'''
								+ CHAR(13)
								+ ''DTEND;TZID=Europe/Dublin:' + CONVERT(VARCHAR,dbo.[convertDateToLocal] (MeetingEndTime,GMTTimeDifference,0),126) + '''
								+ CHAR(13)
								+ ''DTSTART;TZID=Europe/Dublin:' + CONVERT(VARCHAR,dbo.[convertDateToLocal] (MeetingStartTime,GMTTimeDifference,0),126) + '''
								+ CHAR(13)
								+ ''LOCATION:'+ISNULL(C.DCompanyName,'')  +'>> '+ ISNULL(C.DCustGrpName,'')  +'>> '+ ISNULL(C.DivDesc,'') +'>> '+ Location + '''
								+ CHAR(13)
								+ %ATTENDEE%
								+ ''END:VEVENT''
								+ CHAR(13)
								+ ''END:VCALENDAR''
								'
			FROM VPEMeetingHeader VMH WITH(NOLOCK)
			JOIN EGCAPPSDWHS.[dbo].[DBW-SV02-DivisionDescription] C WITH(NOLOCK)
			ON VMH.CONO = C.CONO
			AND VMH.CustGroupID = C.DCustGroupID
			AND VMH.Division = C.Div
			WHERE IMeetingID = @IDVal

			SET @internalAttendee =''
			
			SELECT @internalAttendee +='''ATTENDEE;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION:MAILTO:' +  ParticipantEmailID +'''
			+ CHAR(13) +' FROM [VPEInternalParticipant] NOLOCK WHERE IMeetingID=@IDVal

			SET @Calendarquery = REPLACE(@Calendarquery,'%ATTENDEE%', @internalAttendee)

			SET @EmailCC = '' 
			SET @EmailBCC=''
			
			SET @Subject = 'Internal Meetings -' + @Subject 


		END	
		ELSE IF @EmailType = 17 -- EIM Meeting Participant Availability
		BEGIN 
			 
			SELECT @ParticipantEmailID = ParticipantEmailID,@ParticipantUserName =ParticipantUserName
			FROM [VPEInternalParticipant]
			WHERE IInternalParticipantID = @IParticipantID 
			
			--Get all participants list along with their availability status
			SELECT  ParticipantUserName AS Participant,
			CASE ISNULL(RT.ReferenceKey,0) WHEN 0 THEN 'Notify' ELSE RT.ReferenceText END AS [Availability],
			CASE WHEN ISNULL(RT.ReferenceKey,0) = 3 
			THEN FORMAT(DATEADD(MINUTE,GMTTimeDifference, ProposedStartTime), 'ddd') + '-' + convert(nvarchar(20),DATEADD(MINUTE,GMTTimeDifference, ProposedStartTime),0) + ' (GMT)'
			ELSE '' END AS ProposedTime
			INTO #InternalMeetingAvailability
			FROM [VPEInternalParticipant] I
			LEFT OUTER JOIN [VPEParticipantAvailability] PA
			ON I.IMeetingID = PA.IMeetingID
			AND PA.ParticipantType = 'I'
			AND I.IInternalParticipantID = PA.IParticipantID
			LEFT OUTER JOIN VPEReferences RT WITH(NOLOCK)
			ON RT.ReferenceKey = PA.IAvailabilityStatusID
			AND ReferenceTypeKey = 5
			WHERE I.IMeetingID = @IDVal			
			  
			SET @ParticipantAvailabilityTable = '<table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3" width="90%">'
												+'<tr><td width="35%"><b>Participant</b></td><td width="30%"><b>Status</b></td><td width="35%"><b>Proposed Time</b></td></tr>'

			SELECT @ParticipantAvailabilityTable += '<tr><td>' + Participant + '</td><td>' + [Availability] + '</td><td>' + ProposedTime + '</td></tr>'
			FROM #InternalMeetingAvailability

			SET @ParticipantAvailabilityTable += '</table>'
			
   
			SELECT DISTINCT @Subject = 'Internal Meetings -' + CASE WHEN IAvailabilityStatusID = 1 THEN 'Accepted: ' + MeetingTitle 
										WHEN IAvailabilityStatusID = 2 THEN 'Declined: ' + MeetingTitle 
										ELSE 'New Time Proposed: ' + MeetingTitle  END,
							@HeadedByEmailID = HeadedByEmailID,@CreatedByEmailID =CreatedByEmailID
							,@StrBody = '<font style="font-size:11pt;font-family:calibri;"><br>' + 'Hi ' + CreatedByUserName + ',<br><br>'
							+ @ParticipantUserName +' has ' + CASE WHEN IAvailabilityStatusID=1 THEN 'accepted' WHEN IAvailabilityStatusID=2 THEN 'rejected' ELSE 'proposed new time' END +' for the meeting - ' + MeetingTitle + ' scheduled on '
							+ FORMAT(MeetingStartTime, 'ddd') + ',  ' + convert(nvarchar(20),MeetingStartTime,0)  +' - ' 
							+ CONVERT(varchar(15),CAST(MeetingEndTime AS TIME),100)  + '<br><br>'
							+ CASE WHEN ISNULL(VA.ProposedStartTime,'') <> '' THEN 'Proposed Time:<br> ' + FORMAT(DATEADD(MINUTE,VA.GMTTimeDifference, VA.ProposedStartTime), 'ddd') + '-' + convert(nvarchar(20),DATEADD(MINUTE,VA.GMTTimeDifference, VA.ProposedStartTime),0) + '(GMT)<br><br>' ELSE '' END 
							+ CASE WHEN ISNULL(VA.Comments,'') <> '' THEN 'Comments:<br> ' + VA.Comments + '<br><br>' ELSE '' END + 'Response from all participants: <br>'
							+ @ParticipantAvailabilityTable + '<br>'
							+ '<br>Thanks and regards<br>' + @ParticipantUserName + '</font>'
							FROM VPEMeetingHeader EH
							JOIN [VPEParticipantAvailability] VA
							ON EH.IMeetingID = VA.IMeetingID
							WHERE EH.IMeetingID = @IDVal
							AND VA.IParticipantID = @IParticipantID			

			SET @EmailFrom =@ParticipantEmailID
			SET @replyto = @ParticipantEmailID
			SET @Calendarquery =''
			SET @EmailCC = @HeadedByEmailID
			SET @EmailTo = @CreatedByEmailID	
			
			DROP TABLE #InternalMeetingAvailability		


		END
		ELSE IF @EmailType = 18 -- EIM Meeting Minutes for Participants 
		BEGIN
			--Getting Meeting Minutes Details for internal participant			
			DECLARE @AgendaTableForInternal NVARCHAR(MAX)='',@TaskTableForInternal NVARCHAR(MAX)=''
			
			SET @Calendarquery =''
							
			SELECT DISTINCT
			VMH.IMeetingID
			,ISNULL(MeetingTitle,'') AS MeetingTitle
			,ISNULL(CreatedByUserName,'') AS CreatedByUserName
			,CONVERT(NVARCHAR(20),MeetingStartTime,107) AS MeetingDate
			,CONVERT(varchar(15),CAST(MeetingStartTime AS TIME),100) AS MeetingStartTime
			,CONVERT(varchar(15),CAST(MeetingEndTime AS TIME),100) AS MeetingEndTime
			,ISNULL(C.DCompanyName,'') AS CompanyName 
			,ISNULL(C.DCustGrpName,'') AS CustomerGroup
			,ISNULL(C.DivDesc,'') AS Division
			,ISNULL(HeadedByUserName,'') AS HeadedByUserName
			,ISNULL(Location,'') AS Location
			,ISNULL(VW_IP.InternalParticipantsName,'') AS 'InternalParticipantsName'
			,ISNULL(VW_A.AbsenteeParticipantsName,'') AS 'AbsenteesName'
			INTO #VPEMeetingDetailsForInternal
			FROM VPEMeetingHeader VMH WITH(NOLOCK)
			JOIN EGCAPPSDWHS.[dbo].[DBW-SV02-DivisionDescription] C WITH (NOLOCK)
			ON VMH.CONO = C.CONO
			AND VMH.CustGroupID = C.DCustGroupID
			AND VMH.Division = C.Div			
			LEFT OUTER JOIN EGCAPPSDWHS.dbo.[DBW-SV11-Vendor] V WITH(NOLOCK)
			ON C.CONO = V.CompanyNumber
			AND VMH.VendorNumber = V.VendorNumber			
			LEFT OUTER JOIN VW_VPEMeetingInternalParticipants VW_IP WITH(NOLOCK)
			ON VMH.IMeetingID = VW_IP.IMeetingID
			LEFT OUTER JOIN [VW_VPEAbsentee] VW_A WITH(NOLOCK)
			ON VMH.IMeetingID = VW_A.IMeetingID
			WHERE VMH.IMeetingID = @IDVal 
			AND ISNULL(Inactive,0) = 0

			--Getting Agendas for the Meeting
			SELECT DISTINCT
			CONVERT(NVARCHAR(10),ROW_NUMBER() OVER(ORDER BY VPA.IAgendaID)) + '. ' + ISNULL(AgendaTitle,'') AS AgendaTitle
			,ISNULL(VPN.Notes,'') AS Notes
			,CASE WHEN IsDone=1 THEN 'Completed' ELSE 'Remaining' END AS AgendaStatus
			INTO #VPEAgendasForInternal
			FROM VPEMeetingAgenda VPA
			LEFT OUTER JOIN [VPEMeetingAgendaNotes] VPN WITH(NOLOCK)
			ON VPA.IMeetingId = VPN.IMeetingId
			AND VPA.IAgendaID = VPN.IAgendaID
			WHERE VPA.IMeetingID = @IDVal 
			AND ISNULL(Inactive,0) = 0

			SELECT @AgendaCount = COUNT(*)
								FROM #VPEAgendasForInternal

			--Getting Tasks for the meeting
			SELECT DISTINCT CONVERT(NVARCHAR(10),ROW_NUMBER() OVER(ORDER BY TA.IMeetingTaskID)) +'. ' + TaskTitle AS TaskTitle
			,TaskDesc
			,CONVERT(NVARCHAR(20),StartDate,107) AS StartDate
			,CONVERT(NVARCHAR(20),DueDate,107) AS DueDate
			,ISNULL(VW_TA.TaskAssignedUsersName,'') AS TaskAssignedUsersName
			,VR.ReferenceText as [Priority]
			,VS.ReferenceText as [Status]
			INTO #VPEMeetingTasksForInternal
			FROM VPEMeetingTasks  TA
			JOIN VPEReferences VR WITH(NOLOCK)
			ON TA.ITaskPriorityID = VR.ReferenceKey
			AND VR.ReferenceTypeKey = 3
			JOIN VPEReferences VS WITH(NOLOCK)
			ON TA.ITaskStatusID = VS.ReferenceKey 
			AND VS.ReferenceTypeKey = 2
			LEFT OUTER JOIN VW_VPETaskAssignedUsers VW_TA WITH(NOLOCK)
			ON TA.IMeetingTaskID = VW_TA.IMeetingTaskID	
			WHERE ISNULL(TA.Inactive,0) = 0
			AND TA.IMeetingID IN (SELECT VMH1.IMeetingID	
			FROM VPEMeetingHeader VMH
			INNER JOIN VPEMeetingHeader VMH1
			ON VMH.IRecurrenceID = VMH1.IRecurrenceID
			WHERE VMH.IMeetingID = @IDVal)

			SELECT @TaskCount = COUNT(*)
								FROM #VPEMeetingTasksForInternal

			SELECT @Subject = 'Internal Meetings - Meeting Minutes – ' + MeetingTitle 
							  FROM #VPEMeetingDetailsForInternal N	

			SELECT DISTINCT @StrBody = '<font style="font-size:11pt;font-family:calibri;"><b><font style="font-size:14pt;">' + MeetingTitle +'</font></b><br>' 
									+ 'Created by ' + CreatedByUserName + '<br><br>'
									+ '<table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3" width="95%">'
									+ '<tr><td width="10%"><b>Meeting Date</b></td><td width="30%">' + MeetingDate + ' ' +
									+ MeetingStartTime + ' - ' + MeetingEndTime
									+ '</td><td><b>Room</b></td><td>' + Location  +'</td></tr>'
									+ '<tr><td valign="top"><b>EGC Division</b></td><td>'  + CompanyName  +', '+ CustomerGroup  +', '+ Division + '</td>'
									+ '<td><b>Headed By</b></td><td>' + HeadedByUserName
									+ '</td></tr>'
									+ '<tr><td><b>Participants</b></td><td>' + InternalParticipantsName  +'</td>'
									+ '<td><b>Absentees</b></td><td>' + AbsenteesName  + '</td></tr></table><br>'
			FROM #VPEMeetingDetailsForInternal 
	
			SET @AgendaTableForInternal='<b><font style="font-size:13pt;font-family:calibri;">Agendas</font></b><br>' + '<table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3" width="90%"><tr><td width="20%"><b>Title</b></td><td width="30%"><b>Agenda Comments</b></td><td width="10%"><b>Status</b></td></tr>' 
			SELECT @AgendaTableForInternal +='<tr><td>' + AgendaTitle +'</td><td>' + Notes + '</td><td>' + AgendaStatus + '</td></tr>'
			FROM #VPEAgendasForInternal 

			SET @AgendaTableForInternal += '</table>'

			SET @TaskTableForInternal='<br><b><font style="font-size:13pt;font-family:calibri;">Tasks</font></b><br><br><table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3" width="100%"><tr><td width="20%"><b>Title</b></td><td width="20%"><b>Task Description</b></td><td width="20%"><b>Assigned To</b></td><td width="10%"><b>Start Date</b>'
			+'</td><td width="10%"><b>Due Date</b></td><td width="10%"><b>Priority</b></td><td width="10%"><b>Status</b></td></tr>'
			SELECT @TaskTableForInternal += '<tr><td>' + TaskTitle +'</td><td>' + TaskDesc +  '</td><td>' + TaskAssignedUsersName + '</td><td>' + StartDate + '</td><td>' + DueDate + '</td><td>' + [Priority] + '</td><td>' + [Status] + '</td></tr>'
								FROM #VPEMeetingTasksForInternal

			SET @TaskTableForInternal += '</table>'

			SET @PageURL ='/redirect.aspx?frommode=VPE&ID='

			SELECT @StrBody = @StrBody + CASE WHEN @AgendaCount > 0 THEN @AgendaTableForInternal  ELSE ''  END 
							+ CASE WHEN @TaskCount > 0 THEN @TaskTableForInternal ELSE '' END + ''
							+'<br><a href="' + @siteUrl + @PageURL + CONVERT(NVARCHAR(100),@IDVal) +'">View Meeting Minutes</a><br><br>Thanks and regards<br>' + CreatedByUserName + '</font>'
			FROM #VPEMeetingDetailsForInternal	

			DROP TABLE #VPEMeetingDetailsForInternal	
			DROP TABLE #VPEMeetingTasksForInternal	
			DROP TABLE #VPEAgendasForInternal	


		END 
		ELSE IF @EmailType = 19 -- EIM Cancelled meeting
		BEGIN
			--sending calendar event to participants
			
			SELECT DISTINCT @StrBody = '<font style="font-size:11pt;font-family:calibri;">' 						 
			+ '<br>Meeting scheduled on ' + FORMAT(MeetingStartTime, 'ddd') + ', ' + convert(nvarchar(20),MeetingStartTime,0) 	+
			' - ' + CONVERT(varchar(15),CAST(MeetingEndTime AS TIME),100) + ' has been cancelled.<br></font>'
			,@EmailFrom =CreatedByEmailID,@replyto=CreatedByEmailID
			FROM VPEMeetingHeader
			WHERE IMeetingID = @IDVal
				
			SELECT @Calendarquery ='
								--
								SET NOCOUNT ON;
								--
								SELECT ''BEGIN:VCALENDAR''
								+ CHAR(13)
								+ ''PRODID:-//My Company//Company Calendar//EN''
								+ CHAR(13)
								+ ''VERSION:2.0''
								+ CHAR(13)
								+ ''X-WR-TIMEZONE:Europe/Dublin''
								+ CHAR(13)
								+ ''BEGIN:VTIMEZONE''
								+ CHAR(13)
								+ ''TZID:America/New_York''
								+ CHAR(13)
								+ ''BEGIN:DAYLIGHT''
								+ CHAR(13)
								+ ''TZOFFSETFROM:-0500''
								+ CHAR(13)
								+ ''TZOFFSETTO:-0400''
								+ CHAR(13)
								+ ''DTSTART:' + CONVERT(VARCHAR,dbo.[convertDateToLocal] (MeetingStartTime,GMTTimeDifference,0),126) + '''
								+ CHAR(13)
								+ ''TZNAME:GMT''
								+ CHAR(13)
								+ ''END:DAYLIGHT''
								+ CHAR(13)
								+ ''END:VTIMEZONE''
								+ CHAR(13)
								+ ''METHOD:CANCEL''
								+ CHAR(13)
								+ ''BEGIN:VEVENT''
								+ CHAR(13)
								+ ''CLASS UBLIC''
								+ CHAR(13)
								+ ''DESCRIPTION;FMTTYPE=text/html:<!doctype html><html><body>'+@StrBody+'</body></html>'' 
								+ CHAR(13)
								+ ''X-ALT-DESC;FMTTYPE=text/html:<!doctype html><html><body>'+@StrBody+'</body></html>''
								+ CHAR(13)
								+ ''SUMMARY:'+ MeetingTitle + '''
								+ CHAR(13)
								+''UID:EGCMeetingID' + CONVERT(VARCHAR,IMeetingID) +'''
								+ CHAR(13)
								+ ''DTEND;TZID=Europe/Dublin:' + CONVERT(VARCHAR,dbo.[convertDateToLocal] (MeetingEndTime,GMTTimeDifference,0),126) + '''
								+ CHAR(13)
								+ ''DTSTART;TZID=Europe/Dublin:' + CONVERT(VARCHAR,dbo.[convertDateToLocal] (MeetingStartTime,GMTTimeDifference,0),126) + '''
								+ CHAR(13)
								+ ''LOCATION:'+ISNULL(C.DCompanyName,'')  +'>> '+ ISNULL(C.DCustGrpName,'')  +'>> '+ ISNULL(C.DivDesc,'') +'>> '+ Location + '''
								+ CHAR(13)
								+ %ATTENDEE%
								+ ''END:VEVENT''
								+ CHAR(13)
								+ ''END:VCALENDAR''
								'
			FROM VPEMeetingHeader VMH WITH(NOLOCK)
			JOIN EGCAPPSDWHS.[dbo].[DBW-SV02-DivisionDescription] C WITH(NOLOCK)
			ON VMH.CONO = C.CONO
			AND VMH.CustGroupID = C.DCustGroupID
			AND VMH.Division = C.Div
			WHERE IMeetingID = @IDVal

			SET @internalAttendee =''
			
			SELECT @internalAttendee +='''ATTENDEE;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION:MAILTO:' +  ParticipantEmailID +'''
			+ CHAR(13) +' FROM [VPEInternalParticipant] NOLOCK WHERE IMeetingID=@IDVal

			SET @Calendarquery = REPLACE(@Calendarquery,'%ATTENDEE%', @internalAttendee)

			SET @Subject = 'Internal Meetings -' + @Subject 


		END
		ELSE IF @EmailType = 20 -- EIM Weekly Open Task Report
		BEGIN
			SET @StrBody = @EmailBody
			SET @EmailFrom ='noreply@theegc.com'
			SET @replyto ='noreply@theegc.com'
			SET @Calendarquery =''
		END
			ELSE IF @EmailType = 21 -- EIM Daily Status to user for open tasks
		BEGIN
			SET @StrBody = @EmailBody
			SET @EmailFrom ='noreply@theegc.com'
			SET @replyto ='noreply@theegc.com'
			SET @Calendarquery =''
		END
		ELSE IF @EmailType = 22 --ACD Request- Resend to Requestor 
		BEGIN
			SET @Calendarquery =''
			SET @CreatedByUserName = ''
			SET @PageURL = '/redirect.aspx?frommode=ACD&ID='					

			SELECT @EmailUserName =  LastUpdatedByUserName,@CreatedByUserName = CreatedByUserName,@ACDType=ReqType,@EmailFrom = LastUpdatedByUserEmail
			,@EmailTo = CreatedByUserEmail,@replyto =  LastUpdatedByUserEmail ,@EmailBCC = LastUpdatedByUserEmail,@CONO = CONO,@Division = Division, @CustomerGrp = CustGrp
			FROM ACDRequest
			WHERE IRequestNo = @IDVal

			SELECT @Subject = 'EGC ACD Request #' +CONVERT(NVARCHAR(100),@IDVal) + ' is resent to you by ' + @EmailUserName 

			DELETE FROM @TEMPACDInfo
			INSERT INTO @TEMPACDInfo
			SELECT DISTINCT APA.ACDID,MC.DCompanyName,MC.DivDesc,APA.EGCPartNo,APA.ManufacturerName,APA.ManufacturerPartNo,ISNULL(REPLACE(CPD.CustPartNo,';;',';'),'') AS CustPartNo,
			''
			FROM ACDRequestInfo AR WITH(NOLOCK)
			INNER JOIN ACDPartAttributes APA WITH(NOLOCK)
			ON AR.IACDID = APA.ACDID AND APA.PartType = 1			
			INNER JOIN EGCAPPSDWHS.dbo.[DBW-SV02-DivisionDescription] MC WITH(NOLOCK)
			ON AR.CONO = MC.CONO AND AR.CustGrp  = MC.DCustGroupID AND AR.Division = MC.Div
			LEFT OUTER JOIN VW_ACDCustomerPartDetailsALL CPD WITH(NOLOCK)
			ON APA.ACDID = CPD.ACDID			
			WHERE AR.IRequestNo = @IDVal

			UPDATE ER
			SET ER.IStatusID=4
			FROM @TEMPACDInfo T
			INNER JOIN [PSMI-SQLSTAGE\EXTAPPS].[EGCAPPSDWHSEXT].[dbo].[ProductEnquiryRequests] ER  WITH(NOLOCK)
			ON  T.ACDID = ER.ACDID

			UPDATE T SET T.COMMENTS=AC.COMMENTS
			FROM @TEMPACDInfo T
			INNER JOIN VW_ACDRejectedComments AC
			ON T.ACDID = AC.ACDID
			WHERE AC.CommentType='Sts' AND T.Comments = '' 	

			UPDATE T SET T.COMMENTS=AC.COMMENTS
			FROM @TEMPACDInfo T
			INNER JOIN VW_ACDRejectedComments AC
			ON T.ACDID = AC.ACDID
			WHERE AC.CommentType='Com' AND T.Comments = '' 	
			
			SET @StrBody = '<font style="font-size:11pt;font-family:calibri;">PS- this is an unattended email ID, please do not reply.<br><br>Hi ' + @CreatedByUserName + ',<br><br>Following Item Setup/Update request created by you are either rejected or requires additional clarification.'
		
							
			SET @xmlACDInfo = ''
			SET @bodyACDInfo = ''			

			SET @xmlACDInfo = CAST(( SELECT ACDID AS 'td','',Company AS 'td','',Division AS 'td','',EGCPartNo AS 'td','',CustPartNo AS 'td','',ManufacturerName AS 'td','',ManufacturerPartNo AS 'td','',Comments AS 'td'
			FROM @TEMPACDInfo ORDER BY ACDID DESC 
			FOR XML PATH('tr'), ELEMENTS ) AS NVARCHAR(MAX))

			SET @bodyACDInfo = '<br><br><table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3" width="95%">' +
					'<tr><th> ACDID </th><th> Company </th><th> Division </th><th> EGC Part# </th><th> Customer Part# </th><th> MFG </th><th> MFG Part# </th><th> Comments </th></tr>';
					
			SET @StrBody = @StrBody +  @bodyACDInfo + @xmlACDInfo + '</table>'
			
			SET @StrBody = @StrBody +'<br>Please visit ACD using the link below to review the request.'			
			+'<br><br><a href="' + @siteUrl+ @PageURL + CONVERT(NVARCHAR(100),@IDVal) + '&Gt=me&Type=' + CONVERT(NVARCHAR(10),@ACDType) + '&CONO=' + CONVERT(NVARCHAR(10),@CONO) + '&Division=' + CONVERT(NVARCHAR(10),@Division) +'&CustGrp=' + CONVERT(NVARCHAR(10),@CustomerGrp) + '">Open Request</a><br><br>Thanks<br>EGC Team</font>'

		
		END
		ELSE IF @EmailType = 23 --ACD Request- Approved 
		BEGIN
			DECLARE @IStatusCode1 INT = 0
			SET @CreatedByUserName = ''
			SET @Calendarquery =''	
			
			SET @PageURL = '/redirect.aspx?frommode=ACD&ID='

			SELECT @EmailUserName =  LastUpdatedByUserName,@CreatedByUserName = CreatedByUserName,@ACDType=ReqType,@EmailFrom = LastUpdatedByUserEmail
			,@EmailTo = CreatedByUserEmail,@replyto =  LastUpdatedByUserEmail ,@EmailBCC = LastUpdatedByUserEmail,@CONO = CONO,@Division = Division, @CustomerGrp = CustGrp,
						 
			@IStatusCode1 = IStatusCode
			FROM ACDRequest  WITH(NOLOCK)
			WHERE IRequestNo = @IDVal

			
			DELETE FROM @TEMPACDInfo
			INSERT INTO @TEMPACDInfo
			SELECT DISTINCT APA.ACDID,MC.DCompanyName,MC.DivDesc,APA.EGCPartNo,APA.ManufacturerName,APA.ManufacturerPartNo,ISNULL(REPLACE(CPD.CustPartNo,';;',';'),'') AS CustPartNo,''
			FROM ACDRequestInfo AR WITH(NOLOCK)
			INNER JOIN ACDPartAttributes APA WITH(NOLOCK)
			ON AR.IACDID = APA.ACDID AND APA.PartType = 1
			INNER JOIN EGCAPPSDWHS.dbo.[DBW-SV02-DivisionDescription] MC WITH(NOLOCK)
			ON AR.CONO = MC.CONO AND AR.CustGrp  = MC.DCustGroupID AND AR.Division = MC.Div
			LEFT OUTER JOIN VW_ACDCustomerPartDetailsALL CPD WITH(NOLOCK)
			ON APA.ACDID = CPD.ACDID
			WHERE AR.IRequestNo = @IDVal
			
			IF @IStatusCode1 = 2
			BEGIN
				SET @Subject = 'EGC ACD Request #' +CONVERT(NVARCHAR(100),@IDVal) + ' is approved by ' + @EmailUserName	
				SET @StrBody = '<font style="font-size:11pt;font-family:calibri;">PS- this is an unattended email ID, please do not reply.<br><br>Hi ' + @CreatedByUserName + ',<br><br>Following Item Setup/Update request were approved by ' + @EmailUserName + '.'																			
			END
			ELSE IF @IStatusCode1 = 4
			BEGIN
				SET @Subject = 'EGC ACD Request #' +CONVERT(NVARCHAR(100),@IDVal) + ' is approved by ' + @EmailUserName	
				SET @StrBody = '<font style="font-size:11pt;font-family:calibri;">PS- this is an unattended email ID, please do not reply.<br><br>Hi ' + @CreatedByUserName + ',<br><br>Following Item Setup/Update request were approved by ' + @EmailUserName + '. You will get a confirmation email shortly once the information is updated in Sxe.'
																			
			END
			ELSE IF @IStatusCode1 = 14
			BEGIN
				SET @Subject = 'EGC ACD Request #' +CONVERT(NVARCHAR(100),@IDVal) + ' is approved by ' + @EmailUserName	
				SET @StrBody = '<font style="font-size:11pt;font-family:calibri;">PS- this is an unattended email ID, please do not reply.<br><br>Hi ' + @CreatedByUserName + ',<br><br>Following Item Setup/Update request were approved by '+ @EmailUserName +'. If there is no Inventory approval required, you should get a confirmation email shortly once the information is updated in Sxe.'
																												   
			END			
			SET @xmlACDInfo = ''
			SET @bodyACDInfo = ''			

			SET @xmlACDInfo = CAST(( SELECT ACDID AS 'td','',Company AS 'td','',Division AS 'td','',EGCPartNo AS 'td','',CustPartNo AS 'td','',ManufacturerName AS 'td','',ManufacturerPartNo AS 'td'
			FROM @TEMPACDInfo ORDER BY ACDID DESC 
			FOR XML PATH('tr'), ELEMENTS ) AS NVARCHAR(MAX))

			SET @bodyACDInfo = '<br><br><table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3" width="95%">' +
					'<tr><th> ACDID </th><th> Company </th><th> Division </th><th> EGC Part# </th><th> Customer Part# </th><th> MFG </th><th> MFG Part# </th></tr>';
					
			SET @StrBody = @StrBody +  @bodyACDInfo + @xmlACDInfo + '</table>'
			
			SET @StrBody = @StrBody +'<br>Please visit ACD using the link below to review the request.'
			+'<br><br><a href="' + @siteUrl+ @PageURL + CONVERT(NVARCHAR(100),@IDVal) + '&Gt=as&Type=' + CONVERT(NVARCHAR(10),@ACDType) + '&CONO=' + CONVERT(NVARCHAR(10),@CONO) + '&Division=' + CONVERT(NVARCHAR(10),@Division) +'&CustGrp=' + CONVERT(NVARCHAR(10),@CustomerGrp) + '">Open Request</a><br><br>Thanks<br>EGC Team</font>'
		END
		ELSE IF @EmailType = 24 -- Unprocessed ACDs in SXe(email to IT Team) 
		BEGIN
			SET @EmailTo = NULL	
			SET @EmailCC = NULL	
			SET @EmailBCC = NULL
			SET @StrBody = @EmailBody
			SET @EmailFrom ='noreply@theegc.com'
			SET @replyto ='noreply@theegc.com'
			SET @Calendarquery =''
			SET @Subject = 'ACD -' + @Subject 
		END
		ELSE IF @EmailType = 25 -- ACD Item Setup Done (email to requestor/creator) 
		BEGIN					
   
			SET @StrBody = @EmailBody
			SET @EmailFrom ='noreply@theegc.com'
			SET @replyto ='noreply@theegc.com'
			SET @Calendarquery =''
			SET @Subject = 'ACD -' + @Subject 
		END
			ELSE IF @EmailType = 26 -- Admin PAQ
		BEGIN		
			
			SET @StrBody = @EmailBody
		    SET @EmailFrom ='noreply@theegc.com'
			SET @replyto ='noreply@theegc.com'
			SET @Calendarquery =''
			SET @Subject = 'AdminPAQ -' + @Subject 
		END
		ELSE IF @EmailType = 27 -- Task assigned to supplier mail
		BEGIN	
			SET @PageURL ='/redirect.aspx?frommode=VPETask&ID='
				
			SET @Subject = 'Supplier Collaboration-' + @Subject			

			SELECT DISTINCT @StrBody = '<font style="font-size:11pt;font-family:calibri;">Hi,<br><br> Following Task ' +  + ' has been assigned to you:'+ '<br>'
									+ '<br><table style="font-size:11pt;font-family:calibri;width:48%;" border="1" cellspacing="0" cellpadding="3">'
									+ '<tr><td><b>Task</td></b><td colspan="3">'+ TaskTitle + '</td></tr>'
									+ '<tr><td><b>Description</b></td><td colspan="3" >' + CASE WHEN ISNULL(V.TaskDesc,'')='' THEN 'NA' ELSE ISNULL(V.TaskDesc,'') END + '</td></tr>'
									+ '<tr><td width="15%"><b>Start Date</b></td><td>'+ CONVERT(NVARCHAR(20),V.StartDate,107) + '</td>'
									+ '<td width="15%"><b>Due Date</b></td><td>' + CONVERT(NVARCHAR(20),V.DueDate,107) + '</td></tr>'
									+ '<tr><td><b>Priority</b></td><td>'+ R.ReferenceText + '</td>'
									+ '<td><b>Status</b></td><td>' + VR.ReferenceText + '</td></tr>'
									+ '<tr><td><b>% Completed<b></td><td colspan="3">'+ CONVERT(NVARCHAR(10),V.PercentageCompleted) + '</td></tr>'
									+ '</table>'
			+ '<br>Thanks and regards,<br>' + CASE WHEN H.HeadedByUserName IS NULL THEN V.CreatedByUserName ELSE H.HeadedByUserName END  + '</font>'
			FROM VPEMeetingTasks V
			LEFT OUTER JOIN VPEMeetingHeader H
			ON V.IMeetingID = H.IMeetingID
			JOIN VPEReferences R
			ON V.ITaskPriorityID = R.ReferenceKey
			AND R.ReferenceTypeKey = 3
			JOIN VPEReferences VR
			ON V.ITaskStatusID = VR.ReferenceKey
			AND VR.ReferenceTypeKey = 2
			WHERE IMeetingTaskID = @IDVal	

			SET @Calendarquery =''
			
			SELECT @AttachmentRoot = EmailAttachmentRootDir FROM MasterEmailType WHERE IEmailType=27

			SELECT @EmailAttachment =	STUFF((SELECT DISTINCT ';' + @AttachmentRoot + CONVERT(NVARCHAR(20),IMeetingID) + '\Task\' +   CONVERT(NVARCHAR(20),IMeetingTaskID) + '\' +  ISNULL(VD.[FileName],'')  
												FROM [VPEAttachments] VD WITH(NOLOCK)											
												WHERE IMeetingTaskID = @IDVal FOR XML PATH ('')), 1, 1, '')	

			--SET @StrBody = @StrBody + @LineTable +'</table><br>Please visit Module apps to check the Task and provide your updates using the following link.'
			--+ '<br><br><a href="' + @siteUrl + @PageURL + CONVERT(NVARCHAR(100),@IDVal) +'">Open NC</a><br><br>Thanks<br>EGC Team</font>'
		END
		ELSE IF @EmailType = 28 --Vendor Setup Request- Resend to Requestor 
		BEGIN
			
			SET @Calendarquery =''
			
			SET @PageURL = '/redirect.aspx?frommode=VS&ID='					

			SELECT @Subject = 'EGC Vendor Setup Request #' +CONVERT(NVARCHAR(100),@IDVal) + ' is resent to you by ' + UpdtUNm ,@EmailFrom = UpdtUEml
			,@EmailTo = CrtUEml,@replyto =  UpdtUEml ,@EmailBCC = UpdtUEml,@CONO = CONO,@VSType=ReqTp
			FROM VendorSetupRequest
			WHERE IRequestNo = @IDVal			
			
			SELECT @StrBody = '<font style="font-size:11pt;font-family:calibri;">' +  
									+ '<br>Hi ' + A.CrtUNm +',<br><br>Your ' + CASE A.ReqTp WHEN 'A' THEN 'ADD' WHEN 'Y' THEN 'COPY' WHEN 'C' THEN 'CHANGE' WHEN 'D' THEN 'DELETE' END + ' Request # ' + CONVERT(NVARCHAR(10),@IdVal) + ' created for Company : ' + CO.CompanyName + ' on 
'  
 
   
									+ CONVERT(NVARCHAR(20),A.Crtdt,107)  + ' ' + CONVERT(varchar(15),CAST(A.Crtdt AS TIME),100) 
									+ ' has been resent to you by '+ A.UpdtUNm +'.' +' <br><br>'
									+ 'Total Records : <br>'
									+ '<table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3">'
									+ '<tr><td>Clarification Requested</td><td>'+ CONVERT(NVARCHAR(10),SUM(DISTINCT CASE WHEN GI.IStsCd = 3 THEN 1 ELSE 0 END)) + '</td></tr>'
									+ '<tr><td>Approved</td><td>'+ CONVERT(NVARCHAR(10),SUM(DISTINCT CASE WHEN GI.IStsCd = 4 THEN 1 ELSE 0 END)) + '</td></tr>'
									+ '<tr><td>Rejected</td><td>'+ CONVERT(NVARCHAR(10),SUM(DISTINCT CASE WHEN GI.IStsCd = 7 THEN 1 ELSE 0 END)) + '</td></tr>'
									+ '</table>'
									+'<br>Please visit Module apps to check the Vendor Setup and provide your updates using the following link.'
									+'<br><br><a href="' + @siteUrl+ @PageURL + CONVERT(NVARCHAR(100),@IDVal) + '&Type=' + @VSType + '&CONO=' + CONVERT(NVARCHAR(10),@CONO) + '">Open Request</a><br><br>Thanks<br>EGC Team</font>'
			FROM VendorSetupRequest A WITH (NOLOCK)
			JOIN EGCAPPSDWHS.dbo.[DBW-SV06-Company] CO WITH (NOLOCK)	
			ON A.Cono = CO.CompanyNumber
			JOIN VendorSetupGeneralInfo GI WITH (NOLOCK)
			ON GI.IRequestNo = A.IRequestNo
			WHERE GI.IRequestNo = @IDVal
			GROUP BY A.Crtdt,A.ReqTp,A.CrtUNm,A.UpdtUNm,CO.CompanyName
  
		END
		ELSE IF @EmailType = 29 --Vendor Setup Request- Approved 
		BEGIN
			SET @CrtUNm =''
			SET @Calendarquery =''	
   
			SET @PageURL = '/redirect.aspx?frommode=VS&ID='

			SELECT @EmailUserName =  UpdtUNm,@CrtUNm = CrtUNm,@VSType=ReqTp,@EmailFrom = UpdtUEml
			,@EmailTo = CrtUEml,@replyto =  UpdtUEml ,@EmailBCC = UpdtUEml,@CONO = CONO
			FROM VendorSetupRequest
			WHERE IRequestNo = @IDVal
			
			SET @Subject = 'EGC Vendor Setup Request #' +CONVERT(NVARCHAR(100),@IDVal) + ' is approved by ' + @EmailUserName	

			SET @StrBody = '<font style="font-size:11pt;font-family:calibri;">Hi ' + @CrtUNm + ',<br><br> Vendor Setup Request# ' + CONVERT(NVARCHAR(100),@IDVal) + ' is approved by  ' + @EmailUserName + '.'
			
			SET @StrBody = @StrBody +'<br>Please visit Module apps to check the Vendor Setup and provide your updates using the following link.'
			+'<br><br><a href="' + @siteUrl+ @PageURL + CONVERT(NVARCHAR(100),@IDVal) + '&Type=' + CONVERT(NVARCHAR(10),@VSType) + '&CONO=' + CONVERT(NVARCHAR(10),@CONO) + '">Open Request</a><br><br>Thanks<br>EGC Team</font>'

		END
		ELSE IF @EmailType = 30 -- Unprocessed VSs in SXe(email to IT Team) 
		BEGIN		
			SET @EmailTo = NULL	
			SET @EmailCC = NULL	
			SET @EmailBCC = NULL
			SET @StrBody = @EmailBody
			SET @EmailFrom ='noreply@theegc.com'
			SET @replyto ='noreply@theegc.com'
			SET @Calendarquery =''
			SET @Subject = 'VS -' + @Subject 
		END
		ELSE IF @EmailType = 31 -- Vendor Setup Done (email to requestor/creator) 
		BEGIN		
			
			SET @StrBody = @EmailBody
			SET @EmailFrom ='noreply@theegc.com'
			SET @replyto ='noreply@theegc.com'
			SET @Calendarquery =''
			SET @Subject = 'VS -' + @Subject 
		END
		ELSE IF @EmailType = 32 --Vendor Setup Submitted For First Level Approval
		BEGIN

			SET @CrtUNm =''			
			SET @Calendarquery =''	
			SET @PageURL = '/redirect.aspx?frommode=VS&ID='
		
			SELECT @EmailUserName =  UpdtUNm,@CrtUNm = CrtUNm,@VSType=ReqTp,@EmailFrom = UpdtUEml,
			@replyto =  UpdtUEml ,@EmailBCC = UpdtUEml,@CONO = CONO
			FROM VendorSetupRequest
			WHERE IRequestNo = @IDVal
			
			SELECT @EmailTo = ISNULL(REPLACE(STUFF((SELECT '; ' + UserEmail
					FROM EGCUserNotificationSetup b WITH(NOLOCK)
					WHERE b.CONO = a.CONO 
					AND b.IModuleID = a.IModuleID
					AND b.NotificationType = a.NotificationType
					AND LTRIM(RTRIM(UserEmail)) <> '' 
					FOR XML PATH('')), 1, 2, ''),' ',''),'egcappcritical@megasysindia.com')
				FROM EGCUserNotificationSetup a  WITH(NOLOCK)
				WHERE NotificationType='VendorSetupController'
				and cono = @CONO
				GROUP BY CONO,IModuleID,NotificationType	
			--SET @EmailTo ='egcappcritical@megasysindia.com;';
			SET @Subject = 'EGC Vendor Setup Request #' +CONVERT(NVARCHAR(100),@IDVal) + ' is submitted for approval by ' + @EmailUserName	

			SET @StrBody = '<font style="font-size:11pt;font-family:calibri;">Hi,<br><br> Vendor Setup Request# ' + CONVERT(NVARCHAR(100),@IDVal) + ' is submitted for approval by ' + @EmailUserName + '.'
			
			SET @StrBody = @StrBody +'<br>Please visit Module apps to approve the Vendor Setup request using below link.'
			+'<br><br><a href="' + @siteUrl+ @PageURL + CONVERT(NVARCHAR(100),@IDVal) + '&Type=' + CONVERT(NVARCHAR(10),@VSType) + '&CONO=' + CONVERT(NVARCHAR(10),@CONO) + '">Open Request</a><br><br>Thanks<br>EGC Team</font>'
			
		END
		ELSE IF @EmailType = 33 --Vendor Setup Submitted For Second Level Approval
		BEGIN
			SET @CrtUNm =''			
			SET @Calendarquery =''	
			SET @PageURL = '/redirect.aspx?frommode=VS&ID='

			SELECT @EmailUserName =  UpdtUNm,@CrtUNm = CrtUNm,@VSType=ReqTp,@EmailFrom = UpdtUEml
			,@replyto =  UpdtUEml ,@EmailBCC = UpdtUEml,@CONO = CONO
			FROM VendorSetupRequest
			WHERE IRequestNo = @IDVal
			
			SELECT @EmailTo =  ISNULL(REPLACE(STUFF((SELECT '; ' + UserEmail
					FROM EGCUserNotificationSetup b WITH(NOLOCK)
					WHERE b.CONO = a.CONO 
					AND b.IModuleID = a.IModuleID
					AND b.NotificationType = a.NotificationType
					AND LTRIM(RTRIM(UserEmail)) <> '' 
					FOR XML PATH('')), 1, 2, ''),' ',''),'egcappcritical@megasysindia.com')
				FROM EGCUserNotificationSetup a  WITH(NOLOCK)
				WHERE NotificationType = CASE WHEN CHARINDEX(',' + CONVERT(NVARCHAR(10),@CONO) +',', @VSSecondLevelApporvalCono) >0 THEN 'VendorSetupControllerLevel2' ELSE  'VendorSetupController' END 
				and cono = @CONO
				GROUP BY CONO,IModuleID,NotificationType

			--SET @EmailTo ='egcappcritical@megasysindia.com;'
			SET @Subject = 'EGC Vendor Setup Request #' +CONVERT(NVARCHAR(100),@IDVal) + ' is submitted for approval by ' + @EmailUserName	

			SET @StrBody = '<font style="font-size:11pt;font-family:calibri;">Hi,<br><br> Vendor Setup Request# ' + CONVERT(NVARCHAR(100),@IDVal) + ' is submitted for approval by ' + @EmailUserName + '.'
			
			SET @StrBody = @StrBody +'<br>Please visit Module apps to approve the Vendor Setup request using below link.'
			+'<br><br><a href="' + @siteUrl+ @PageURL + CONVERT(NVARCHAR(100),@IDVal) + '&Type=' + CONVERT(NVARCHAR(10),@VSType) + '&CONO=' + CONVERT(NVARCHAR(10),@CONO) + '">Open Request</a><br><br>Thanks<br>EGC Team</font>'
		END
		ELSE IF @EmailType = 35 --ACD Submitted For Approval
		BEGIN
		    DECLARE @CEN INT = 0
			DECLARE @IStatusCode INT = 0
			SET @CrtUNm =''			
			SET @Calendarquery =''	
			SET @PageURL = '/redirect.aspx?frommode=ACD&ID='

			SELECT @EmailUserName = LastUpdatedByUserName,@CrtUNm = CreatedByUserName,@ACDType=ReqType,@EmailFrom = LastUpdatedByUserEmail,
			@replyto =  LastUpdatedByUserEmail,@EmailBCC = LastUpdatedByUserEmail,
			@CONO = CONO,@Division = Division, @CustomerGrp = CustGrp, @IStatusCode = IStatusCode			
			FROM ACDRequest WITH(NOLOCK)
			WHERE IRequestNo = @IDVal	

			SET @EmailTo ='';
			IF @IStatusCode = 2
			BEGIN
				SELECT @CEN=ControllerEmailNotification 
				FROM ACDAdminSetup WITH(NOLOCK)
				WHERE CONO=@CONO
				AND Div=@Division
				AND CustGrp=@CustomerGrp
				IF @CEN=1
				BEGIN
					SELECT @EmailTo =  ISNULL(REPLACE(STUFF((SELECT '; ' + UserEmail
						FROM EGCUserNotificationSetup b WITH(NOLOCK)
						WHERE b.CONO = a.CONO 
						AND b.IModuleID = a.IModuleID
						AND b.NotificationType = a.NotificationType
						AND LTRIM(RTRIM(UserEmail)) <> '' 
						FOR XML PATH('')), 1, 2, ''),' ',''),'egcappcritical@megasysindia.com')
					FROM EGCUserNotificationSetup a  WITH(NOLOCK)
					WHERE IModuleID = 1
					AND NotificationType = 'ACDController'
					AND CONO = @CONO
					GROUP BY CONO,IModuleID,NotificationType
				END
				ELSE BEGIN
					SET @EmailTo ='';
				END
				DELETE FROM @TEMPACDInfo
				INSERT INTO @TEMPACDInfo
				SELECT DISTINCT APA.ACDID,MC.DCompanyName,MC.DivDesc,APA.EGCPartNo,APA.ManufacturerName,APA.ManufacturerPartNo,ISNULL(REPLACE(CPD.CustPartNo,';;',';'),'') AS CustPartNo,''
				FROM ACDRequestInfo AR WITH(NOLOCK)
				INNER JOIN ACDPartAttributes APA WITH(NOLOCK)
				ON AR.IACDID = APA.ACDID AND APA.PartType = 1
				INNER JOIN EGCAPPSDWHS.dbo.[DBW-SV02-DivisionDescription] MC WITH(NOLOCK)
				ON AR.CONO = MC.CONO AND AR.CustGrp  = MC.DCustGroupID AND AR.Division = MC.Div
				LEFT OUTER JOIN VW_ACDCustomerPartDetailsALL CPD WITH(NOLOCK)
				ON APA.ACDID = CPD.ACDID
				WHERE AR.IRequestNo = @IDVal
				AND Active = 1
				AND IStatusCode = 2
			END
			ELSE IF @IStatusCode = 8
			BEGIN 
				--for the site manager approval hot email will be sent
				SET @Importance = 'High'				
				SELECT @EmailTo =  ISNULL(REPLACE(STUFF((SELECT '; ' + WorkEmail
					FROM EGCAPPSDWHS.[dbo].[mstrSA006Site_Plant_Information] SP WITH(NOLOCK)
					INNER JOIN EGCIntranetData.[dbo].[EGCUserProfileData] EPD WITH(NOLOCK)
					ON SP.SiteMGR = EPD.AccountName			
					WHERE SP.CONO = @CONO
					AND SP.DivNo = @Division 
					FOR XML PATH('')), 1, 2, ''),' ',''),'egcappcritical@megasysindia.com')

					DELETE FROM @TEMPACDInfo
					INSERT INTO @TEMPACDInfo
					SELECT DISTINCT APA.ACDID,MC.DCompanyName,MC.DivDesc,APA.EGCPartNo,APA.ManufacturerName,APA.ManufacturerPartNo,ISNULL(REPLACE(CPD.CustPartNo,';;',';'),'') AS CustPartNo,''
					FROM ACDRequestInfo AR WITH(NOLOCK)
					INNER JOIN ACDPartAttributes APA WITH(NOLOCK)
					ON AR.IACDID = APA.ACDID AND APA.PartType = 1
					INNER JOIN EGCAPPSDWHS.dbo.[DBW-SV02-DivisionDescription] MC WITH(NOLOCK)
					ON AR.CONO = MC.CONO AND AR.CustGrp  = MC.DCustGroupID AND AR.Division = MC.Div
					LEFT OUTER JOIN VW_ACDCustomerPartDetailsALL CPD WITH(NOLOCK)
					ON APA.ACDID = CPD.ACDID
					WHERE AR.IRequestNo = @IDVal
					AND Active = 1
					AND IStatusCode = 8
			END
			ELSE IF @IStatusCode = 14
			BEGIN
				SELECT @EmailTo =  ISNULL(REPLACE(STUFF((SELECT '; ' + UserEmail
						FROM EGCUserNotificationSetup b WITH(NOLOCK)
						WHERE b.CONO = a.CONO 
						AND b.IModuleID = a.IModuleID
						AND b.NotificationType = a.NotificationType
						AND LTRIM(RTRIM(UserEmail)) <> '' 
						FOR XML PATH('')), 1, 2, ''),' ',''),'egcappcritical@megasysindia.com')
					FROM EGCUserNotificationSetup a  WITH(NOLOCK)
					WHERE IModuleID = 27
					AND NotificationType = 'ACDControllerLevel2'
					AND CONO = @CONO
					GROUP BY CONO,IModuleID,NotificationType

					DELETE FROM @TEMPSumMinMax
					INSERT INTO @TEMPSumMinMax
					SELECT DISTINCT APA.ACDID,MC.DCompanyName,MC.DivDesc,APA.EGCPartNo,ISNULL(REPLACE(CPD.CustPartNo,';;',';'),'') AS CustPartNo,0 AS CMin,0 AS CMax,RM.ActualMin,RM.ActualMax,APA.PartType
					FROM ACDRequestInfo AR WITH(NOLOCK)
					INNER JOIN ACDPartAttributes APA WITH(NOLOCK)
					ON AR.IACDID = APA.ACDID AND APA.PartType = 1
					INNER JOIN EGCAPPSDWHS.dbo.[DBW-SV02-DivisionDescription] MC WITH(NOLOCK)
					ON AR.CONO = MC.CONO AND AR.CustGrp  = MC.DCustGroupID AND AR.Division = MC.Div
					INNER JOIN ACDWarehouseRecMinMax RM WITH(NOLOCK)
					ON RM.ACDID = AR.IACDID AND RM.PartType  = APA.PartType 
					LEFT OUTER JOIN VW_ACDCustomerPartDetailsALL CPD WITH(NOLOCK)
					ON APA.ACDID = CPD.ACDID
					WHERE AR.IRequestNo = @IDVal
					AND Active = 1
					AND IStatusCode = 14

					UPDATE T
					SET CMin = AW.MinQtySum,
					CMax = AW.MaxQtySum
					FROM @TEMPSumMinMax T
					INNER JOIN (SELECT ACDID, PartType, SUM(ISNULL(MinQty,0)) AS MinQtySum, SUM(ISNULL(MaxQty,0)) AS MaxQtySum
					FROM ACDWarehouse_OldData W WITH(NOLOCK) WHERE W.WarehouseType IN ('CRIB','VENDING') AND W.WarehouseStatus='S' AND PartType = 1  
					GROUP BY ACDID, PartType) AW
					ON T.ACDID = AW.ACDID AND T.PartType = AW.PartType
				END

			
			--SET @EmailTo ='egcappcritical@megasysindia.com;';
			SET @Subject = 'EGC ACD Request #' +CONVERT(NVARCHAR(100),@IDVal) + ' is submitted for ' + CASE WHEN  @IStatusCode = 8 THEN 'site'  WHEN  @IStatusCode = 14 THEN 'Inventory' ELSE '' END + ' approval by ' + @EmailUserName	

			SET @StrBody = '<font style="font-size:11pt;font-family:calibri;">PS- this is an unattended email ID, please do not reply.<br><br>Hi,<br><br>Following Item Setup/Update request were submitted for ' + CASE WHEN  @IStatusCode = 8 THEN 'site'  WHEN  @IStatusCode = 14 THEN 'Inventory' ELSE '' END
 + ' approval by ' + @EmailUserName + '.'
			
			SET @xmlACDInfo = ''
			SET @bodyACDInfo = ''
			
			IF (@IStatusCode IN (2,8))
			BEGIN

				SET @xmlACDInfo = CAST(( SELECT ACDID AS 'td','',Company AS 'td','',Division AS 'td','',EGCPartNo AS 'td','',CustPartNo AS 'td','',ManufacturerName AS 'td','',ManufacturerPartNo AS 'td'
				FROM @TEMPACDInfo ORDER BY ACDID DESC 
				FOR XML PATH('tr'), ELEMENTS ) AS NVARCHAR(MAX))

				SET @bodyACDInfo = '<br><br><table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3" width="95%">' +
						'<tr><th> ACDID </th><th> Company </th><th> Division </th><th> EGC Part# </th><th> Customer Part# </th><th> MFG </th><th> MFG Part# </th></tr>';
			END
			ELSE IF @IStatusCode = 14
			BEGIN
				SET @xmlACDInfo = CAST(( SELECT ACDID AS 'td','',Company AS 'td','',Division AS 'td','',EGCPartNo AS 'td','',CustPartNo AS 'td','',CONVERT(NVARCHAR(10),CMin) + '/' + CONVERT(NVARCHAR(10),CMax) AS 'td','',CONVERT(NVARCHAR(10),NMin) + '/' + CONVERT(NVARCHAR(10),NMax) AS 'td'
				FROM @TEMPSumMinMax ORDER BY ACDID DESC 
				FOR XML PATH('tr'), ELEMENTS ) AS NVARCHAR(MAX))

				SET @bodyACDInfo = '<br><br><table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3" width="95%">' +
						'<tr><th> ACDID </th><th> Company </th><th> Division </th><th> EGC Part# </th><th> Customer Part# </th><th> Min/Max(Current) </th><th> Min/Max(requested)</th></tr>';
			
			END

			SET @StrBody = @StrBody +  @bodyACDInfo + @xmlACDInfo + '</table>'

			SET @StrBody = @StrBody +'<br>Please visit ACD using the link below to review the request.'
			+'<br><br><a href="' + @siteUrl+ @PageURL + CONVERT(NVARCHAR(100),@IDVal) + '&Gt=as&Type=' + CONVERT(NVARCHAR(10),@ACDType) + '&CONO=' + CONVERT(NVARCHAR(10),@CONO) + '&Division=' + CONVERT(NVARCHAR(10),@Division) +'&CustGrp=' + CONVERT(NVARCHAR(10),@CustomerGrp) + '">Open Request</a><br><br>Thanks<br>EGC Team</font>'
			
		
		END
		ELSE IF @EmailType = 37 -- Vending Warehouse Change(email to SiteManagers of divison) 
		BEGIN		
			SET @EmailTo ='egcappsupportstage@megasysindia.com;';
			SET @StrBody = @EmailBody
			SET @EmailFrom ='noreply@theegc.com'
			SET @replyto ='noreply@theegc.com'
			SET @Calendarquery =''
			SET @Subject = 'ACD -' + @Subject 
		END
		ELSE IF @EmailType = 36 -- Customer Batch Item Setup Done (email to requestor/creator) 
		BEGIN					
   
			SET @StrBody = @EmailBody
			SET @EmailFrom ='noreply@theegc.com'
			SET @replyto ='noreply@theegc.com'
			SET @Calendarquery =''
			SET @Subject =  @Subject 
		END
		ELSE IF @EmailType IN (38,39,40,41,42,43,44) --Operations email
		BEGIN	
			SET @EmailBCC = ''
			SET @Subject =  @Subject
			SET @Importance = 'High'
			SET @StrBody = @EmailBody
			SET @EmailFrom ='noreply@theegc.com'
			SET @replyto ='noreply@theegc.com'
			SET @Calendarquery =''			
			SET @StrBody = @EmailBody 

							
		END
		ELSE IF @EmailType in(101,102) --CSDBApps
		BEGIN	
			SET @EmailBCC = 'nrao@psmicorp.com;dbangera@psmicorp.com;'
			SET @Subject =  @Subject
			SET @Importance = 'High'
			SET @StrBody = @EmailBody
			SET @Calendarquery =''	
			SET @EmailAttachment = ''

		END
		ELSE IF @EmailType IN (45,46,47,48,49,50,51,52,53,58,59,61,64) --VPI 
		BEGIN
			SET @EmailTo ='vsawant@psmicorp.com;kkupate@psmicorp.com;dmargaj@psmicorp.com';
			SET @EmailBCC = 'vsawant@psmicorp.com;'
			SET @Subject =  @Subject			
			SET @StrBody = @EmailBody
			SET @Calendarquery =''	
			SET @EmailAttachment = ''
		END
		ELSE IF @EmailType = 55 -- ACD Approve item with price change has open sales order 
		BEGIN		
			--SET @EmailBCC = 'nsamani@psmicorp.com;'
							 
			SET @StrBody = @EmailBody
			SET @EmailFrom ='noreply@theegc.com'
			SET @replyto ='noreply@theegc.com'
			SET @Calendarquery =''
			SET @Subject = 'ACD -' + @Subject 
	   
		END		
		---------------------------------------Recursive Meeting Start-----------------------------------------
		ELSE IF @EmailType = 65 -- VPE Calendar invite to all participants For Recursive Meeting -CREATE 16
		BEGIN
			--sending calendar event to participants
		
		
		print ('Newrecursive')
		SELECT @SupplierPortalEnabled=dbo.fn_IsVendorSupplierPortalEnbaled(@IDVal,'Meeting')
			

				SELECT DISTINCT @EmailFrom =CreatedByEmailID,@ReplyTo=CreatedByEmailID,
							@EmailAttachment =	STUFF((SELECT DISTINCT ';' + ISNULL(VD.FilePath,'')  
												FROM VPEDivisionDocuments VD WITH(NOLOCK)											
												WHERE VD.CONO = VMH.CONO
												AND VD.Division =  VMH.Division FOR XML PATH ('')), 1, 1, '')																		
			FROM VPEMeetingHeader VMH
			JOIN EGCAPPSDWHS.[dbo].[DBW-SV02-DivisionDescription] C WITH (NOLOCK)
			ON VMH.CONO = C.CONO
			AND VMH.CustGroupID = C.DCustGroupID
			AND VMH.Division = C.Div		
			JOIN EGCAPPSDWHS.dbo.[DBW-SV11-Vendor] V WITH(NOLOCK)
			ON C.CONO = V.CompanyNumber
			AND VMH.VendorNumber = V.VendorNumber		
			LEFT OUTER JOIN VW_VPEMeetingInternalParticipants VW_IP
			ON VMH.IMeetingID = VW_IP.IMeetingID
			LEFT OUTER JOIN VW_VPEMeetingExternalParticipants VW_EP
			ON VMH.IMeetingID = VW_EP.IMeetingID 			
			WHERE VMH.IMeetingID = @IDVal
			AND ISNULL(Inactive,0) = 0	




			Select  @StrBody=dbo.Fn_SetRecursiveContent(65,@IDVal,'')
			DECLARE @recinfo AS NVARCHAR(MAX)
			DECLARE @recmsg AS NVARCHAR(MAX)
			DECLARE @ReplacePageURLTemp AS NVARCHAR(MAX)
			SELECT @recinfo=recuringinfo FROM VPEMeetingHeader WHERE IMeetingID=@IDVal
			
			SELECT @recmsg=VALUE FROM OPENJSON(@recinfo) ref WHERE ref.[key]='message'
			SET @Subject=ISNULL(@Subject,'')+' ' +ISNULL(@recmsg,'');
			SET @Calendarquery =dbo.fn_GetCalanderQueryForRecursiveMeeting(@IDVal,@StrBody)
			
			IF @SupplierPortalEnabled = 1 AND @RecipientType = 'E' 
			BEGIN

			DECLARE @meetingplantinfo AS NVARCHAR(MAX)
			Select  @meetingplantinfo=ISNULL(dbo.Fn_SetRecursiveContent(0,@IDVal,'PLANTSAFETY'),'')
			 SELECT @StrBody=REPLACE(@StrBody,'ReplacePlantSafetyInfo',@meetingplantinfo)
			 print ('rew recursive supplier portal')
			       ---newrecursivemeeting
				INSERT INTO [PSMI-SQLSTAGE\EXTAPPS].Supplierportal.dbo.EmailNotificationLog(IdVal,IsProcessed,EmailType,EmailSubject,EmailBody,EmailTo,LogOnDatettime,CalendarQuery)
				SELECT @IDVal,0,6,@Subject,@StrBody,@EmailTo,GETDATE(),@Calendarquery

				UPDATE EmailNotificationLog
				SET IsProcessed = 1
				WHERE IEmailSrNo=@SrNo

			END
			ELSE
			BEGIN

			print 'non supplierportal enabled'
			IF @RecipientType = 'I'
					BEGIN
						SELECT @IParticipantID = ISNULL(IInternalParticipantID,'')
						FROM [VPEInternalParticipant]
						WHERE IMeetingID = @IDVal
						AND LTRIM(RTRIM(ParticipantEmailID)) = LTRIM(RTRIM(@EmailTo))
					END
				ELSE
					BEGIN
						SELECT @IParticipantID =  ISNULL(IExternalParticipantID,'')
						FROM [VPEExternalParticipant]
						WHERE IMeetingID = @IDVal
						AND LTRIM(RTRIM(ParticipantEmailID)) = LTRIM(RTRIM(@EmailTo))
				END


				
				IF ISNULL(@ParticipantEmailID,0) <>0
				BEGIN
				  SET @PageURL ='/LandingAuth.aspx?frommode=VPEAvailability&ID=' + CONVERT(NVARCHAR(100),@IDVal) 
							 + '&IParticipantID=' + CONVERT(NVARCHAR(100),@IParticipantID) + '&ParticipantType=' + @RecipientType	
			
						 SET @ReplacePageURLTemp=@VPESiteUrl + @PageURL
						 SELECT @StrBody=REPLACE(@StrBody,'ReplacePageURLTemp',@ReplacePageURLTemp)
						 SELECT @StrBody=REPLACE(@StrBody,'ReplacePlantSafetyInfo','')
				END
				ELSE
				BEGIN
				        SELECT @StrBody=REPLACE(@StrBody,'ReplacePageURLTemp','')
						 SELECT @StrBody=REPLACE(@StrBody,'ReplacePlantSafetyInfo','')

				END
		   
			 

			print 'non supplierportal enabled'
			print @StrBody
			SET @Subject = 'Supplier Collaboration-' + @Subject
			END

			
			

		END
		ELSE IF @EmailType = 66 -- VPE Calendar Create Meeting With Excluded Date  -CREATE 11
		BEGIN
			--sending calendar event to participants
						
						SELECT @IParticipantID = IInternalParticipantID
			FROM [VPEInternalParticipant]
			WHERE IMeetingID = @IDVal
			AND LTRIM(RTRIM(ParticipantEmailID)) = LTRIM(RTRIM(@EmailTo)) 

			--Redirecting users to EIM's Landing Auth page that doesnt required any authentication

			SET @PageURL ='/LandingAuth.aspx?frommode=VPEAvailability&ID=' + CONVERT(NVARCHAR(100),@IDVal) 
						 + '&IParticipantID=' + CONVERT(NVARCHAR(100),@IParticipantID) + '&ParticipantType=' + @RecipientType + '&IM=Y'			
						
			SELECT DISTINCT @EmailFrom =CreatedByEmailID,@replyto=CreatedByEmailID,
								@EmailAttachment =	STUFF((SELECT DISTINCT ';' + ISNULL(VD.FilePath,'')  
												FROM VPEDivisionDocuments VD WITH(NOLOCK)											
												WHERE VD.CONO = VMH.CONO
												AND VD.Division =  VMH.Division FOR XML PATH ('')), 1, 1, '')																		
			FROM VPEMeetingHeader VMH
			JOIN EGCAPPSDWHS.[dbo].[DBW-SV02-DivisionDescription] C WITH (NOLOCK)
			ON VMH.CONO = C.CONO
			AND VMH.CustGroupID = C.DCustGroupID
			AND VMH.Division = C.Div		
			LEFT OUTER JOIN EGCAPPSDWHS.dbo.[DBW-SV11-Vendor] V WITH(NOLOCK)
			ON C.CONO = V.CompanyNumber
			AND VMH.VendorNumber = V.VendorNumber		
			LEFT OUTER JOIN VW_VPEMeetingInternalParticipants VW_IP
			ON VMH.IMeetingID = VW_IP.IMeetingID	
			WHERE VMH.IMeetingID = @IDVal
			AND ISNULL(Inactive,0) = 0		
		
		
			Select  @StrBody=dbo.Fn_SetRecursiveContent(66,@IDVal,'')
		  
			SET @Calendarquery =dbo.fn_GetCalanderQueryWithExcludedDateforRecursiveMeeting(@IDVal,@StrBody)

			SET @EmailCC = ''
			SET @EmailBCC= ''
			SET @Subject = 'Supplier Collaboration-' + @Subject

		END
		ELSE IF @EmailType = 67 -- VPE Calendar Recursive Meeting Cancel the Whole Series -DELETE 19
		BEGIN
			SELECT @SupplierPortalEnabled=dbo.fn_IsVendorSupplierPortalEnbaled(@IDVal,'Meeting')
	
			SELECT DISTINCT @EmailFrom =CreatedByEmailID,@replyto=CreatedByEmailID
			FROM VPEMeetingHeader
			WHERE IMeetingID = @IDVal

			Select  @StrBody=dbo.Fn_SetRecursiveContent(67,@IDVal,'')
		  
			SET @Calendarquery =dbo.fn_GetCalanderQueryForCancelRecursiveMeeting(@IDVal,@StrBody)

			SET @internalAttendee =''
			
			SELECT @internalAttendee +='''ATTENDEE;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION:MAILTO:' +  ParticipantEmailID +'''
			+ CHAR(13) +' FROM [VPEInternalParticipant] NOLOCK WHERE IMeetingID=@IDVal

			SET @Calendarquery = REPLACE(@Calendarquery,'%ATTENDEE%', @internalAttendee)

			IF @SupplierPortalEnabled = 1 
			BEGIN
			

			 print ('cancel recursive supplier portal')
			       ---newrecursivemeeting
				INSERT INTO [PSMI-SQLSTAGE\EXTAPPS].Supplierportal.dbo.EmailNotificationLog(IdVal,IsProcessed,EmailType,EmailSubject,EmailBody,EmailTo,LogOnDatettime,CalendarQuery)
				SELECT @IDVal,0,7,@Subject,@StrBody,@EmailTo,GETDATE(),@Calendarquery	
			
			
				
			END


			
			SET @Subject = 'Supplier Collaboration-' + @Subject

		END
		ELSE IF @EmailType = 68 -- VPE  remove --internalAttendee & External Attendee 13
		BEGIN
			--sending calendar event to participants
			SELECT @SupplierPortalEnabled = ISNULL(V.SupplierPortalEnabled,0)
			FROM VPEMeetingHeader VMH WITH(NOLOCK)			
			JOIN EGCAPPSDWHS.dbo.[DBW-SV11-Vendor] V WITH(NOLOCK)
			ON VMH.CONO= V.CompanyNumber
			AND VMH.VendorNumber = V.VendorNumber
			WHERE VMH.IMeetingID = @IDVal
			AND V.CompanyNumber NOT IN (SELECT CompanyNumber FROM EGCAppsDWHS.dbo.mstrInvalidCompanies)
			AND ISNULL(Inactive,0) = 0	

			SET @EmailTo = REPLACE(@EmailTo,',',';')
			--Insert into Supplier Portal Email log if supplier is supplier portal enabled.
			IF @SupplierPortalEnabled = 1 BEGIN
			
				INSERT INTO [PSMI-SQLSTAGE\EXTAPPS].Supplierportal.dbo.EmailNotificationLog(IdVal,EmailType,EmailTo,LogOnDatettime)
				SELECT @IDVal,5,ParticipantEmailID,GETDATE()
				FROM [VPEExternalParticipant] NOLOCK 
				WHERE imeetingID=@IDVal
				
			END

			SELECT DISTINCT @EmailTo =  STUFF((
											SELECT ';' + u.ParticipantEmailID
											FROM VPEInternalParticipant u
											WHERE IMeetingID=@IDVal
											ORDER BY u.ParticipantUserName
											FOR XML PATH('')
											),1,1,'') 

			SELECT DISTINCT @StrBody = '<font style="font-size:11pt;font-family:calibri;">' 
			+ '<br>Meeting scheduled on ' + FORMAT(MeetingStartTime, 'ddd') + ',  ' + convert(nvarchar(20),MeetingStartTime,0) 	+
			
			' - ' + CONVERT(varchar(15),CAST(MeetingEndTime AS TIME),100) + ' has been cancelled.<br>please open the attachment and click on "Remove from Calendar" to remove meeting from your calendar.</font>'
			,@EmailFrom =CreatedByEmailID,@ReplyTo=CreatedByEmailID
			FROM VPEMeetingHeader
			WHERE IMeetingID = @IDVal
				
			SELECT @Calendarquery ='
								--
								SET NOCOUNT ON;
								--
								SELECT ''BEGIN:VCALENDAR''
								+ CHAR(13)
								+ ''PRODID:-//My Company//Company Calendar//EN''
								+ CHAR(13)
								+ ''VERSION:2.0''
								+ CHAR(13)
								+ ''X-WR-TIMEZONE:Europe/Dublin''
								+ CHAR(13)
								+ ''BEGIN:VTIMEZONE''
								+ CHAR(13)
								+ ''TZID:America/New_York''
								+ CHAR(13)
								+ ''BEGIN:DAYLIGHT''
								+ CHAR(13)
								+ ''TZOFFSETFROM:-0500''
								+ CHAR(13)
								+ ''TZOFFSETTO:-0400''
								+ CHAR(13)
								+ ''DTSTART:' + CONVERT(VARCHAR,dbo.[convertDateToLocal] (MeetingStartTime,GMTTimeDifference,0),126) + '''
								+ CHAR(13)
								+ ''TZNAME:GMT''
								+ CHAR(13)
								+ ''END:DAYLIGHT''
								+ CHAR(13)
								+ ''END:VTIMEZONE''
								+ CHAR(13)
								+ ''METHOD:CANCEL''
								+ CHAR(13)
								+ ''BEGIN:VEVENT''
								+ CHAR(13)
								+ ''CLASS:PUBLIC''
								+ CHAR(13)
								+ ''DESCRIPTION;FMTTYPE=text/html:<!doctype html><html><body>'+@StrBody+'</body></html>'' 
								+ CHAR(13)
								+ ''X-ALT-DESC;FMTTYPE=text/html:<!doctype html><html><body>'+@StrBody+'</body></html>''
								+ CHAR(13)
								+ ''SUMMARY:'+ MeetingTitle + '''
								+ CHAR(13)
								+''UID:EGCMeetingID' + CONVERT(VARCHAR,IMeetingID) +'''
								+ CHAR(13)
								+ ''ORGANIZER;CN=' + HeadedByUserName +':mailto:' + HeadedByEmailID +'''
								+ CHAR(13)
								+ ''DTEND;TZID=Europe/Dublin:' + CONVERT(VARCHAR,dbo.[convertDateToLocal] (MeetingEndTime,GMTTimeDifference,0), 126) + '''
								+ CHAR(13)
								+ ''DTSTART;TZID=Europe/Dublin:' + CONVERT(VARCHAR,dbo.[convertDateToLocal] (MeetingStartTime,GMTTimeDifference,0), 126) + '''
								+ CHAR(13)
								+ ''LOCATION:'+ISNULL(C.DCompanyName,'')  +'>> '+ ISNULL(C.DCustGrpName,'')  +'>> '+ ISNULL(C.DivDesc,'') +'>> '+ Location + '''
								+ CHAR(13)
								+ %ATTENDEE%
								+ ''END:VEVENT''
								+ CHAR(13)
								+ ''END:VCALENDAR''
								'
			FROM VPEMeetingHeader VMH WITH(NOLOCK)
			JOIN EGCAPPSDWHS.[dbo].[DBW-SV02-DivisionDescription] C WITH(NOLOCK)
			ON VMH.CONO = C.CONO
			AND VMH.CustGroupID = C.DCustGroupID
			AND VMH.Division = C.Div
			WHERE IMeetingID = @IDVal

			SET @internalAttendee =''
			SET @externalAttendee =''

			SELECT @internalAttendee +='''ATTENDEE;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION:MAILTO:' +  ParticipantEmailID +'''
			+ CHAR(13) +' FROM [VPEInternalParticipant] NOLOCK WHERE imeetingID=@IDVal

			SELECT @externalAttendee +='''ATTENDEE;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION:MAILTO:' +  ParticipantEmailID +'''
			+ CHAR(13) +' FROM [VPEExternalParticipant] NOLOCK WHERE imeetingID=@IDVal

			SET @Calendarquery = REPLACE(@Calendarquery,'%ATTENDEE%', @internalAttendee + @externalAttendee)

			SET @Subject = 'Supplier Collaboration-' + @Subject 
		

		END
		ELSE IF @EmailType=69  --Recursive Meeting Occurance Update
		BEGIN			SELECT DISTINCT @EmailFrom =CreatedByEmailID,@replyto=CreatedByEmailID
			FROM VPEMeetingHeader
			WHERE IMeetingID = @IDVal

			Select  @StrBody=dbo.Fn_SetRecursiveContent(69,@IDVal,'')
		  
			SET @Calendarquery =dbo.Fn_UpdateOccuranceOfRecursiveSeries(@IDVal,@StrBody)

			SET @internalAttendee =''
			
			SELECT @internalAttendee +='''ATTENDEE;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION:MAILTO:' +  ParticipantEmailID +'''
			+ CHAR(13) +' FROM [VPEInternalParticipant] NOLOCK WHERE IMeetingID=@IDVal

			SET @Calendarquery = REPLACE(@Calendarquery,'%ATTENDEE%', @internalAttendee)

			SELECT @SupplierPortalEnabled=dbo.fn_IsVendorSupplierPortalEnbaled(@IDVal,'Meeting')

			IF @SupplierPortalEnabled = 1 AND @RecipientType = 'E' 
			BEGIN

			 print ('rew recursive supplier portal')
			       ---newrecursivemeeting
				INSERT INTO [PSMI-SQLSTAGE\EXTAPPS].Supplierportal.dbo.EmailNotificationLog(IdVal,IsProcessed,EmailType,EmailSubject,EmailBody,EmailTo,LogOnDatettime,CalendarQuery)
				SELECT @IDVal,0,8,@Subject,@StrBody,@EmailTo,GETDATE(),@Calendarquery

				UPDATE EmailNotificationLog
				SET IsProcessed = 1
				WHERE IEmailSrNo=@SrNo

				SET @StrBody = ''
			END
			ELSE
			BEGIN

			SET @EmailCC = ''
			SET @EmailBCC= ''
			SET @Subject = 'Supplier Collaboration-' + @Subject

			print @Calendarquery
			END

		END
		ELSE IF @EmailType=70  --Recursive Meeting Updated Series Cancellation
		BEGIN			

			SELECT DISTINCT @EmailFrom =CreatedByEmailID,@replyto=CreatedByEmailID
			FROM VPEMeetingHeader
			WHERE IMeetingID = @IDVal

			Select  @StrBody=dbo.Fn_SetRecursiveContent(70,@IDVal,'')
		  
			SET @Calendarquery =dbo.fn_GetCalanderQueryForCancelUpdatedRecursiveMeeting(@IDVal,@StrBody)
			
			SET @internalAttendee =''
			
			SELECT @internalAttendee +='''ATTENDEE;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION:MAILTO:' +  ParticipantEmailID +'''
			+ CHAR(13) +' FROM [VPEInternalParticipant] NOLOCK WHERE IMeetingID=@IDVal

			SET @Calendarquery = REPLACE(@Calendarquery,'%ATTENDEE%', @internalAttendee)

			--Series Cancellation recursive meeting and Insert into SupplierPortal Databse
			SELECT @SupplierPortalEnabled=dbo.fn_IsVendorSupplierPortalEnbaled(@IDVal,'Meeting')

			IF @SupplierPortalEnabled = 1
			BEGIN

			    INSERT INTO [PSMI-SQLSTAGE\EXTAPPS].Supplierportal.dbo.EmailNotificationLog(IdVal,IsProcessed,EmailType,EmailSubject,EmailBody,EmailTo,LogOnDatettime,CalendarQuery)
				SELECT @IDVal,0,7,@Subject,@StrBody,@EmailTo,GETDATE(),@Calendarquery

			END

			SET @EmailCC = ''
			SET @EmailBCC= ''
			SET @Subject = 'Supplier Collaboration-' + @Subject

		END
		ELSE IF @EmailType=71  --Recursive Meeting Cancel Single Occurance
		BEGIN			

			SELECT DISTINCT @EmailFrom =CreatedByEmailID,@replyto=CreatedByEmailID
			FROM VPEMeetingHeader
			WHERE IMeetingID = @IDVal

			Select  @StrBody=dbo.Fn_SetRecursiveContent(71,@IDVal,'')
		  
			SET @Calendarquery =dbo.fn_GetCalanderQueryForCancelOccuranceRecursiveMeeting(@IDVal,@StrBody)

			SET @internalAttendee =''
			
			SELECT @internalAttendee +='''ATTENDEE;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION:MAILTO:' +  ParticipantEmailID +'''
			+ CHAR(13) +' FROM [VPEInternalParticipant] NOLOCK WHERE IMeetingID=@IDVal

			SET @Calendarquery = REPLACE(@Calendarquery,'%ATTENDEE%', @internalAttendee)

			--Cancelled recursive meeting and Insert into SupplierPortal Databse
			SELECT @SupplierPortalEnabled=dbo.fn_IsVendorSupplierPortalEnbaled(@IDVal,'Meeting')

			IF @SupplierPortalEnabled = 1
			BEGIN

			    INSERT INTO [PSMI-SQLSTAGE\EXTAPPS].Supplierportal.dbo.EmailNotificationLog(IdVal,IsProcessed,EmailType,EmailSubject,EmailBody,EmailTo,LogOnDatettime,CalendarQuery)
				SELECT @IDVal,0,7,@Subject,@StrBody,@EmailTo,GETDATE(),@Calendarquery

			END
			
			SET @EmailCC = ''
			SET @EmailBCC= ''
			SET @Subject = 'Supplier Collaboration-' + @Subject
						
		END

		ELSE IF @EmailType = 404 -- VPE Headed by User
		BEGIN

			SELECT @SupplierPortalEnabled = ISNULL(V.SupplierPortalEnabled,0)
			FROM VPEMeetingHeader VMH WITH(NOLOCK)			
			JOIN EGCAPPSDWHS.dbo.[DBW-SV11-Vendor] V WITH(NOLOCK)
			ON VMH.CONO= V.CompanyNumber
			AND VMH.VendorNumber = V.VendorNumber
			WHERE VMH.IMeetingID = @IDVal
			AND V.CompanyNumber NOT IN (SELECT CompanyNumber FROM EGCAppsDWHS.dbo.mstrInvalidCompanies)
			AND ISNULL(Inactive,0) = 0	

			--Insert into Supplier Portal Email log if supplier is supplier portal enabled.
			IF @SupplierPortalEnabled = 1 AND @RecipientType = 'E' BEGIN
			
				INSERT INTO [PSMI-SQLSTAGE\EXTAPPS].Supplierportal.dbo.EmailNotificationLog(IdVal,EmailType,EmailTo,LogOnDatettime)
				SELECT @IDVal,4,@EmailTo,GETDATE()

				UPDATE EmailNotificationLog
				SET IsProcessed = 1
				WHERE IEmailSrNo=@SrNo

				--Update EmailTo to blank to not process this email
				SET @EmailTo =''

			END
			ELSE BEGIN
				--sending calendar event to participants
				IF @RecipientType = 'I'
					BEGIN
						SELECT @IParticipantID = ISNULL(IInternalParticipantID,'')
						FROM [VPEInternalParticipant]
						WHERE IMeetingID = @IDVal
						AND LTRIM(RTRIM(ParticipantEmailID)) = LTRIM(RTRIM(@EmailTo))
					END
				ELSE
					BEGIN
						SELECT @IParticipantID =  ISNULL(IExternalParticipantID,'')
						FROM [VPEExternalParticipant]
						WHERE IMeetingID = @IDVal
						AND LTRIM(RTRIM(ParticipantEmailID)) = LTRIM(RTRIM(@EmailTo))
				END

				--Redirecting users to VPE's Landing Auth page that doent required any authentication

				SET @PageURL ='/LandingAuth.aspx?frommode=VPEAvailability&ID=' + CONVERT(NVARCHAR(100),@IDVal) 
							 + '&IParticipantID=' + CONVERT(NVARCHAR(100),@IParticipantID) + '&ParticipantType=' + @RecipientType	
							 
				SET @strPgUrl = 'To confirm your participation, please click on the link below. If you would like to inform any change or non-participation please reply to this email<br>'
										+ '<a href="' + @VPESiteUrl + @PageURL +'">Please click here to confirm your availability</a><br>'
						
				SELECT DISTINCT @StrBody = '<font style="font-size:11pt;font-family:calibri;">' +  
										+ '<br>Hi,<br><br>You are invited for a meeting. The details are given below. The meeting invite is attached with this email,' 
										+ 'please open the attachment and save it to show it in your calendar.<br><br><font >Meeting Subject - ' + MeetingTitle
										+'</font><br><br>' + '<table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3" width="95%">'
										+ '<tr><td width="10%"><b>Meeting Date</b></td><td width="30%">' + CONVERT(NVARCHAR(20),MeetingStartTime,107)  + ' ' +
				 
										+ CONVERT(varchar(15),CAST(MeetingStartTime AS TIME),100)	
										+ ' - ' + CONVERT(varchar(15),CAST(MeetingEndTime AS TIME),100) 
										+ '</td><td><b>Room</b></td><td>' + Location  +'</td></tr>'
										+ '<tr><td valign="top"><b>Supplier</b></td><td>' + CONVERT(NVARCHAR(10),V.VendorNumber)+' - '+ V.OrigVendName + '</td>'
										+ '<td><b>EGC Division</b></td><td>' + ISNULL(C.DCompanyName,'')  +', '+ ISNULL(C.DCustGrpName,'')  +', '+ ISNULL(C.DivDesc,'') 
										+ '</td></tr>'
										+ '<tr><td><b>Headed By</b></td><td>' + ISNULL(HeadedByUserName,'') +'</td></tr>'
										+ '</table><br>' +
										CASE WHEN ISNULL(@IParticipantID,'') = '' THEN '' ELSE @strPgUrl END -- Sending mail to Headed by and not creating acceptance link 
										+ '<br>Thanks and regards<br>' + CreatedByUserName + '</font>',
								@EmailFrom =CreatedByEmailID,@ReplyTo=CreatedByEmailID,
								@EmailAttachment =	STUFF((SELECT DISTINCT ';' + ISNULL(VD.FilePath,'')  
													FROM VPEDivisionDocuments VD WITH(NOLOCK)											
													WHERE VD.CONO = VMH.CONO
													AND VD.Division =  VMH.Division FOR XML PATH ('')), 1, 1, '')																		
				FROM VPEMeetingHeader VMH
				JOIN EGCAPPSDWHS.[dbo].[DBW-SV02-DivisionDescription] C WITH (NOLOCK)
				ON VMH.CONO = C.CONO
				AND VMH.CustGroupID = C.DCustGroupID
				AND VMH.Division = C.Div		
				JOIN EGCAPPSDWHS.dbo.[DBW-SV11-Vendor] V WITH(NOLOCK)
				ON C.CONO = V.CompanyNumber
				AND VMH.VendorNumber = V.VendorNumber		
				LEFT OUTER JOIN VW_VPEMeetingInternalParticipants VW_IP
				ON VMH.IMeetingID = VW_IP.IMeetingID
				LEFT OUTER JOIN VW_VPEMeetingExternalParticipants VW_EP
				ON VMH.IMeetingID = VW_EP.IMeetingID 			
				WHERE VMH.IMeetingID = @IDVal
				AND ISNULL(Inactive,0) = 0		
				
				SELECT @Calendarquery ='
									--
									SET NOCOUNT ON;
									--
									SELECT ''BEGIN:VCALENDAR''
									+ CHAR(13)
									+ ''PRODID:-//My Company//Company Calendar//EN''
									+ CHAR(13)
									+ ''VERSION:2.0''
									+ CHAR(13)
									+ ''X-WR-TIMEZONE:Europe/Dublin''
									+ CHAR(13)
									+ ''BEGIN:VTIMEZONE''
									+ CHAR(13)
									+ ''TZID:America/New_York''
									+ CHAR(13)
									+ ''BEGIN:DAYLIGHT''
									+ CHAR(13)
									+ ''TZOFFSETFROM:-0500''
									+ CHAR(13)
									+ ''TZOFFSETTO:-0400''
									+ CHAR(13)
									+ ''DTSTART:' + CONVERT(VARCHAR,dbo.[convertDateToLocal] (MeetingStartTime,GMTTimeDifference,0),126) + '''
									+ CHAR(13)
									+ ''TZNAME:GMT''
									+ CHAR(13)
									+ ''END:DAYLIGHT''
									+ CHAR(13)
									+ ''END:VTIMEZONE''
									+ CHAR(13)
									+ ''METHOD:PUBLISH''
									+ CHAR(13)
									+ ''BEGIN:VEVENT''
									+ CHAR(13)
									+ ''CLASS:PUBLIC''
									+ CHAR(13)
									+ ''DESCRIPTION;FMTTYPE=text/html:<!doctype html><html><body>'+@StrBody+'</body></html>'' 
									+ CHAR(13)
									+ ''X-ALT-DESC;FMTTYPE=text/html:<!doctype html><html><body>'+@StrBody+'</body></html>''
									+ CHAR(13)
									+ ''SUMMARY:'+ MeetingTitle  + '''
									+ CHAR(13)
									+ ''UID:EGCMeetingID' + CONVERT(VARCHAR,IMeetingID) +'''
									+ CHAR(13)
									+ ''ORGANIZER;CN=' + HeadedByUserName +':mailto:' + HeadedByEmailID +'''
									+ CHAR(13)
									+ ''DTEND;TZID=Europe/Dublin:' + CONVERT(VARCHAR,dbo.[convertDateToLocal] (MeetingEndTime,GMTTimeDifference,0),126) + '''
									+ CHAR(13)
									+ ''DTSTART;TZID=Europe/Dublin:' + CONVERT(VARCHAR,dbo.[convertDateToLocal] (MeetingStartTime,GMTTimeDifference,0),126) + '''
									+ CHAR(13)
									+ ''LOCATION:'+ISNULL(C.DCompanyName,'')  +'>> '+ ISNULL(C.DCustGrpName,'')  +'>> '+ ISNULL(C.DivDesc,'') +'>> '+ Location + '''
									+ CHAR(13)
									+ %ATTENDEE%
									+ ''END:VEVENT''
									+ CHAR(13)
									+ ''END:VCALENDAR''
									'
				FROM VPEMeetingHeader VMH WITH(NOLOCK)
				JOIN EGCAPPSDWHS.[dbo].[DBW-SV02-DivisionDescription] C WITH(NOLOCK)
				ON VMH.CONO = C.CONO
				AND VMH.CustGroupID = C.DCustGroupID
				AND VMH.Division = C.Div
				WHERE IMeetingID = @IDVal

				SET @internalAttendee =''
				SET @externalAttendee =''
				SELECT @internalAttendee +='''ATTENDEE;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION;RSVP=TRUE:MAILTO:' +  ParticipantEmailID +'''
				+ CHAR(13) +' FROM [VPEInternalParticipant] NOLOCK WHERE imeetingID=@IDVal

				SELECT @externalAttendee +='''ATTENDEE;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION;RSVP=TRUE:MAILTO:' +  ParticipantEmailID +'''
				+ CHAR(13) +' FROM [VPEExternalParticipant] NOLOCK WHERE imeetingID=@IDVal

				SET @Calendarquery = REPLACE(@Calendarquery,'%ATTENDEE%', @internalAttendee + @externalAttendee)

				SET @EmailCC = ''
				SET @EmailBCC= ''
				SET @Subject = 'Supplier Collaboration-' + @Subject
			END
		END
		---------------------------------------Recursive Meeting Ends-----------------------------------------
		ELSE IF @EmailType = 305 -- VPE Calendar invite to all participants
		BEGIN
			--sending calendar event to participants
			--DECLARE @CustomerParticipants NVARCHAR(255)

			--SELECT @CustomerParticipants = STUFF((SELECT ', ' + EP.ParticipantEmailID 
			--							FROM VPEMeetingHeader MH
			--							JOIN VPEMeetingCustomerParticipants EP
			--							ON EP.IMeetingID = MH.IMeetingID
			--							AND EP.IMeetingID = @IDVal ORDER BY EP.ParticipantEmailID
			--							FOR XML PATH ('')), 1, 1, '') 		
						
			SELECT DISTINCT @StrBody = '<font style="font-size:11pt;font-family:calibri;">' +  
									+ '<br>Hi,<br><br>You are invited for a meeting. The details are given below. The meeting invite is attached with this email,' 
									+ 'please open the attachment and save it to show it in your calendar.<br><br><font >Meeting Subject - ' + MeetingTitle
									+'</font><br><br>' + '<table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3" width="95%">'
									+ '<tr><td width="10%"><b>Meeting Date</b></td><td width="30%">' + CONVERT(NVARCHAR(20),MeetingStartTime,107)  + ' ' +
									+ CONVERT(varchar(15),CAST(MeetingStartTime AS TIME),100)	
									+ ' - ' + CONVERT(varchar(15),CAST(MeetingEndTime AS TIME),100) 
									+ '</td><td><b>Room</b></td><td>' + Location  +'</td></tr>'
									+ '<tr><td valign="top"><b>Supplier</b></td><td>' + CONVERT(NVARCHAR(10),V.VendorNumber)+' - '+ V.OrigVendName + '</td>'
									+ '<td><b>EGC Division</b></td><td>' + ISNULL(C.DCompanyName,'')  +', '+ ISNULL(C.DCustGrpName,'')  +', '+ ISNULL(C.DivDesc,'') 
									+ '</td></tr>'
									+ '<tr><td><b>Headed By</b></td><td>' + ISNULL(HeadedByUserName,'') +'</td>'
									+'<td><b>Participants</b></td><td>' + ISNULL(VW_IP.InternalParticipantsName,'') + ',' + ISNULL(VW_EP.ExternalParticipantsName,'')
									+ '</td></tr>'
									+ '</table><br>'									
									+ '<br>Thanks and regards<br>' + CreatedByUserName + '</font>',
							@EmailFrom =CreatedByEmailID,@ReplyTo=CreatedByEmailID,
							@EmailAttachment =	STUFF((SELECT DISTINCT ';' + ISNULL(VD.FilePath,'')  
												FROM VPEDivisionDocuments VD WITH(NOLOCK)											
												WHERE VD.CONO = VMH.CONO
												AND VD.Division =  VMH.Division FOR XML PATH ('')), 1, 1, '')																		
			FROM VPEMeetingHeader VMH
			JOIN EGCAPPSDWHS.[dbo].[DBW-SV02-DivisionDescription] C WITH (NOLOCK)
			ON VMH.CONO = C.CONO
			AND VMH.CustGroupID = C.DCustGroupID
			AND VMH.Division = C.Div		
			JOIN EGCAPPSDWHS.dbo.[DBW-SV11-Vendor] V WITH(NOLOCK)
			ON C.CONO = V.CompanyNumber
			AND VMH.VendorNumber = V.VendorNumber		
			LEFT OUTER JOIN VW_VPEMeetingInternalParticipants VW_IP
			ON VMH.IMeetingID = VW_IP.IMeetingID
			LEFT OUTER JOIN VW_VPEMeetingExternalParticipants VW_EP
			ON VMH.IMeetingID = VW_EP.IMeetingID 			
			WHERE VMH.IMeetingID = @IDVal
			AND ISNULL(Inactive,0) = 0		
				
			SELECT @Calendarquery ='
								--
								SET NOCOUNT ON;
								--
								SELECT ''BEGIN:VCALENDAR''
								+ CHAR(13)
								+ ''PRODID:-//My Company//Company Calendar//EN''
								+ CHAR(13)
								+ ''VERSION:2.0''
								+ CHAR(13)
								+ ''X-WR-TIMEZONE:Europe/Dublin''
								+ CHAR(13)
								+ ''BEGIN:VTIMEZONE''
								+ CHAR(13)
								+ ''TZID:America/New_York''
								+ CHAR(13)
								+ ''BEGIN:DAYLIGHT''
								+ CHAR(13)
								+ ''TZOFFSETFROM:-0500''
								+ CHAR(13)
								+ ''TZOFFSETTO:-0400''
								+ CHAR(13)
								+ ''DTSTART:' + CONVERT(VARCHAR,dbo.[convertDateToLocal] (MeetingStartTime,GMTTimeDifference,0),126) + '''
								+ CHAR(13)
								+ ''TZNAME:GMT''
								+ CHAR(13)
								+ ''END:DAYLIGHT''
								+ CHAR(13)
								+ ''END:VTIMEZONE''
								+ CHAR(13)
								+ ''METHOD:PUBLISH''
								+ CHAR(13)
								+ ''BEGIN:VEVENT''
								+ CHAR(13)
								+ ''CLASS:PUBLIC''
								+ CHAR(13)
								+ ''DESCRIPTION;FMTTYPE=text/html:<!doctype html><html><body>'+@StrBody+'</body></html>'' 
								+ CHAR(13)
								+ ''X-ALT-DESC;FMTTYPE=text/html:<!doctype html><html><body>'+@StrBody+'</body></html>''
								+ CHAR(13)
								+ ''SUMMARY:'+ MeetingTitle  + '''
								+ CHAR(13)
								+ ''UID:EGCMeetingID' + CONVERT(VARCHAR,IMeetingID) +'''
								+ CHAR(13)
								+ ''ORGANIZER;CN=' + HeadedByUserName +':mailto:' + HeadedByEmailID +'''
								+ CHAR(13)
								+ ''DTEND;TZID=Europe/Dublin:' + CONVERT(VARCHAR,dbo.[convertDateToLocal] (MeetingEndTime,GMTTimeDifference,0),126) + '''
								+ CHAR(13)
								+ ''DTSTART;TZID=Europe/Dublin:' + CONVERT(VARCHAR,dbo.[convertDateToLocal] (MeetingStartTime,GMTTimeDifference,0),126) + '''
								+ CHAR(13)
								+ ''LOCATION:'+ISNULL(C.DCompanyName,'')  +'>> '+ ISNULL(C.DCustGrpName,'')  +'>> '+ ISNULL(C.DivDesc,'') +'>> '+ Location + '''
								+ CHAR(13)
								+ %ATTENDEE%
								+ ''END:VEVENT''
								+ CHAR(13)
								+ ''END:VCALENDAR''
								'
			FROM VPEMeetingHeader VMH WITH(NOLOCK)
			JOIN EGCAPPSDWHS.[dbo].[DBW-SV02-DivisionDescription] C WITH(NOLOCK)
			ON VMH.CONO = C.CONO
			AND VMH.CustGroupID = C.DCustGroupID
			AND VMH.Division = C.Div
			WHERE IMeetingID = @IDVal

			SET @internalAttendee =''
			SET @externalAttendee =''
			SET @customerAttendee =''
			SELECT @internalAttendee +='''ATTENDEE;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION;RSVP=TRUE:MAILTO:' +  ParticipantEmailID +'''
			+ CHAR(13) +' FROM [VPEInternalParticipant] NOLOCK WHERE imeetingID=@IDVal

			SELECT @externalAttendee +='''ATTENDEE;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION;RSVP=TRUE:MAILTO:' +  ParticipantEmailID +'''
			+ CHAR(13) +' FROM [VPEExternalParticipant] NOLOCK WHERE imeetingID=@IDVal

			SELECT @customerAttendee +='''ATTENDEE;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION;RSVP=TRUE:MAILTO:' +  ParticipantEmailID +'''
			+ CHAR(13) +' FROM VPEMeetingCustomerParticipants NOLOCK WHERE imeetingID=@IDVal

			SET @Calendarquery = REPLACE(@Calendarquery,'%ATTENDEE%', @internalAttendee + @externalAttendee + @customerAttendee)

			SET @EmailCC = ''
			SET @EmailBCC= ''
			SET @Subject = 'Supplier Collaboration-' + @Subject

		END
		ELSE IF @EmailType = 60 -- VPE Meeting Request from Supplier Portal
		BEGIN
			SET @Calendarquery =''
			--SA-346 : Approve Meeting from email
			SET @PageURL ='/redirect.aspx?frommode=Os6GtvEqk17=&ID=' + CONVERT(NVARCHAR(100),@IDVal) 
				
			SELECT DISTINCT @StrBody = '<font style="font-size:11pt;font-family:calibri;">' +  
			+ '<br>Hi,<br><br>A new meeting request is raised by supplier user '+RequestorUserName+'.<br><br><font >Title - ' + MeetingTitle
			+'</font><br><br>' + '<table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3" width="95%">'
			+ '<tr><td width="10%"><b>Date</b></td><td width="30%">' + CONVERT(NVARCHAR(20),MeetingStartTime,107)  + ' ' +
			+ CONVERT(varchar(15),CAST(MeetingStartTime AS TIME),100)    
			+ ' - ' + CONVERT(varchar(15),CAST(MeetingEndTime AS TIME),100) 
			+ '</td><td><b>Room</b></td><td>' + Location  +'</td></tr>'
			+ '<tr><td valign="top"><b>Supplier</b></td><td>' + CONVERT(NVARCHAR(10),V.VendorNumber)+' - '+ V.OrigVendName + '</td>'
			+ '<td><b>EGC Division</b></td><td>' + ISNULL(C.DCompanyName,'')  +', '+ ISNULL(C.DCustGrpName,'')  +', '+ ISNULL(C.DivDesc,'') 
			+ '</td></tr>'
			+ '<tr><td><b>Headed By</b></td><td>' + ISNULL(HeadedByUserName,'') +'</td>'
			+'<td><b>Participants</b></td><td>' + ISNULL(VW_IP.InternalParticipantsName,'') + ',' + ISNULL(VW_EP.ExternalParticipantsName,'') + '</td></tr>'
			+ '</table><br>' 
			+'<br><a href="' + @siteUrl + @PageURL +'">Please click here to approve or reject the meeting request</a><br>'
			+'<br>Regards,<br>Team EGC</font>'
			,@ReplyTo=RequestorEmailID,@EmailTo = HeadedByEmailID,@EmailFrom='noreply@theegc.com'
			,@Subject='Supplier Collaboration - New Meeting Request By '+V.OrigVendName
			FROM VPEMeetingRequestHeader VMH WITH(NOLOCK)
			JOIN EGCAPPSDWHS.[dbo].[DBW-SV02-DivisionDescription] C WITH (NOLOCK)
			ON VMH.CONO = C.CONO
			AND VMH.CustGroupID = C.DCustGroupID
			AND VMH.Division = C.Div        
			JOIN EGCAPPSDWHS.dbo.[DBW-SV11-Vendor] V WITH(NOLOCK)
			ON C.CONO = V.CompanyNumber
			AND VMH.VendorNumber = V.VendorNumber        
			LEFT OUTER JOIN VW_VPEMeetingRequestInternalParticipants VW_IP WITH(NOLOCK)
			ON VMH.Id = VW_IP.Id
			LEFT OUTER JOIN VW_VPEMeetingRequestExternalParticipants VW_EP WITH(NOLOCK)
			ON VMH.Id = VW_EP.Id             
			WHERE VMH.Id = @IDVal	

			--Add all internal participant in recipient list
			SELECT @EmailTO += ';' + ParticipantEmailID  
			FROM VPEMeetingRequestInternalParticipants WITH(NOLOCK) 
			WHERE RequestID = @IDVal AND ParticipantEmailID <> @EmailTO

			SET @EmailTo = @EmailTo
			SET @EmailBCC ='nrao@psmicorp.com'

		END
		ELSE IF @EmailType = 308 -- Update meeting details by supplier
		BEGIN
			DECLARE @VPEMeetingAgendaTable NVARCHAR(MAX)=''
			--Getting Agendas for the Meeting
			SELECT
			CONVERT(NVARCHAR(10),ROW_NUMBER() OVER(ORDER BY VPA.IAgendaID)) + '. ' + ISNULL(AgendaTitle,'') AS AgendaTitle
			,ISNULL(VPN.Notes,'') AS Notes
			,CASE WHEN IsDone=1 THEN 'Completed' ELSE 'Remaining' END AS AgendaStatus
			INTO #VPEMeetingDetailsAgendas
			FROM VPEMeetingAgenda VPA
			LEFT OUTER JOIN [VPEMeetingAgendaNotes] VPN WITH(NOLOCK)
			ON VPA.IMeetingId = VPN.IMeetingId
			AND VPA.IAgendaID = VPN.IAgendaID
			WHERE VPA.IMeetingID = @IDVal 
			AND ISNULL(Inactive,0) = 0

			SELECT @AgendaCount = COUNT(*)
								FROM #VPEMeetingDetailsAgendas	
								
			SET @VPEMeetingAgendaTable='<b><font style="font-size:13pt;font-family:calibri;">Agendas</font></b><br>' + '<table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3" width="90%"><tr><td width="20%"><b>Title</b></td><td width="30%"><b>Agenda Comments</b></td><td width="10%"><b>Status</b></td></tr>' 
			SELECT @VPEMeetingAgendaTable +='<tr><td>' + AgendaTitle +'</td><td>' + Notes + '</td><td>' + AgendaStatus + '</td></tr>'
			FROM #VPEMeetingDetailsAgendas 

			SET @VPEMeetingAgendaTable += '</table>'
						
			SELECT DISTINCT @StrBody = '<font style="font-size:11pt;font-family:calibri;">' +  
									+ '<br>Hi,<br><br>Below meeting has been updated by supplier. <br><br><font >Meeting Subject - ' + MeetingTitle
									+'</font><br><br>' + '<table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3" width="95%">'
									+ '<tr><td width="10%"><b>Meeting Date</b></td><td width="30%">' + CONVERT(NVARCHAR(20),MeetingStartTime,107)  + ' ' +
				 
									+ CONVERT(varchar(15),CAST(MeetingStartTime AS TIME),100)	
									+ ' - ' + CONVERT(varchar(15),CAST(MeetingEndTime AS TIME),100) 
									+ '</td><td><b>Room</b></td><td>' + Location  +'</td></tr>'
									+ '<tr><td valign="top"><b>Supplier</b></td><td>' + CONVERT(NVARCHAR(10),V.VendorNumber)+' - '+ V.OrigVendName + '</td>'
									+ '<td><b>EGC Division</b></td><td>' + ISNULL(C.DCompanyName,'')  +', '+ ISNULL(C.DCustGrpName,'')  +', '+ ISNULL(C.DivDesc,'') 
									+ '</td></tr>'
									+ '<tr><td><b>Headed By</b></td><td>' + ISNULL(HeadedByUserName,'') +'</td>'
									+'<td><b>Participants</b></td><td>' + ISNULL(VW_IP.InternalParticipantsName,'') + ',' + ISNULL(VW_EP.ExternalParticipantsName,'') + '</td></tr>'
									+ '</table><br>'+CASE WHEN @AgendaCount > 0 THEN @VPEMeetingAgendaTable  ELSE ''  END  +'<br><br>'
									+ '<br>Thanks<br>EGC Team</font>'	,
									@Subject = 'Meeting Details Updated By The Supplier - ' + MeetingTitle
			FROM VPEMeetingHeader VMH
			JOIN EGCAPPSDWHS.[dbo].[DBW-SV02-DivisionDescription] C WITH (NOLOCK)
			ON VMH.CONO = C.CONO
			AND VMH.CustGroupID = C.DCustGroupID
			AND VMH.Division = C.Div		
			JOIN EGCAPPSDWHS.dbo.[DBW-SV11-Vendor] V WITH(NOLOCK)
			ON C.CONO = V.CompanyNumber
			AND VMH.VendorNumber = V.VendorNumber		
			LEFT OUTER JOIN VW_VPEMeetingInternalParticipants VW_IP
			ON VMH.IMeetingID = VW_IP.IMeetingID
			LEFT OUTER JOIN VW_VPEMeetingExternalParticipants VW_EP
			ON VMH.IMeetingID = VW_EP.IMeetingID 			
			WHERE VMH.IMeetingID = @IDVal
			AND ISNULL(Inactive,0) = 0		
				
			SET @Calendarquery =''
			
			SET @EmailCC = ''
			SET @EmailBCC= ''
			SET @EmailFrom ='noreply@theegc.com'  -- TBA need to be discussed with Vishram and Sreenath

			
		END
		ELSE IF @EmailType = 309 --VPE Meeting Minutes Updated by supplier
		BEGIN
			--Getting Meeting Minutes Details for internal participant			
			DECLARE @VPEAgendaTable NVARCHAR(MAX)='',@VPETaskTable NVARCHAR(MAX)=''
			
			SET @Calendarquery =''
							
			SELECT DISTINCT
			VMH.IMeetingID
			,ISNULL(MeetingTitle,'') AS MeetingTitle
			,ISNULL(CreatedByUserName,'') AS CreatedByUserName
			,CONVERT(NVARCHAR(20),MeetingStartTime,107) AS MeetingDate
			,CONVERT(varchar(15),CAST(MeetingStartTime AS TIME),100) AS MeetingStartTime
			,CONVERT(varchar(15),CAST(MeetingEndTime AS TIME),100) AS MeetingEndTime
			,ISNULL(C.DCompanyName,'') AS CompanyName 
			,ISNULL(C.DCustGrpName,'') AS CustomerGroup
			,ISNULL(C.DivDesc,'') AS Division
			,CONVERT(NVARCHAR(10),V.VendorNumber)+' - '+ V.OrigVendName AS Supplier
			,ISNULL(HeadedByUserName,'') AS HeadedByUserName
			,ISNULL(Location,'') AS Location
			,ISNULL(VW_IP.InternalParticipantsName,'') AS 'InternalParticipantsName'
			,ISNULL(VW_EP.ExternalParticipantsName,'') AS 'ExternalParticipantsName'
			,ISNULL(VW_A.AbsenteeParticipantsName,'') AS 'AbsenteesName'
			INTO #VPEMeetingMinutesDetails
			FROM VPEMeetingHeader VMH WITH(NOLOCK)
			JOIN EGCAPPSDWHS.[dbo].[DBW-SV02-DivisionDescription] C WITH (NOLOCK)
			ON VMH.CONO = C.CONO
			AND VMH.CustGroupID = C.DCustGroupID
			AND VMH.Division = C.Div			
			JOIN EGCAPPSDWHS.dbo.[DBW-SV11-Vendor] V WITH(NOLOCK)
			ON C.CONO = V.CompanyNumber
			AND VMH.VendorNumber = V.VendorNumber			
			LEFT OUTER JOIN VW_VPEMeetingInternalParticipants VW_IP WITH(NOLOCK)
			ON VMH.IMeetingID = VW_IP.IMeetingID
			LEFT OUTER JOIN VW_VPEMeetingExternalParticipants VW_EP WITH(NOLOCK)
			ON VMH.IMeetingID = VW_EP.IMeetingID
			LEFT OUTER JOIN [VW_VPEAbsentee] VW_A WITH(NOLOCK)
			ON VMH.IMeetingID = VW_A.IMeetingID
			WHERE VMH.IMeetingID = @IDVal 
			AND ISNULL(Inactive,0) = 0

			--Getting Agendas for the Meeting
			SELECT
			CONVERT(NVARCHAR(10),ROW_NUMBER() OVER(ORDER BY VPA.IAgendaID)) + '. ' + ISNULL(AgendaTitle,'') AS AgendaTitle
			,ISNULL(VPN.Notes,'') AS Notes
			,CASE WHEN IsDone=1 THEN 'Completed' ELSE 'Remaining' END AS AgendaStatus
			INTO #VPEMeetingAgendas
			FROM VPEMeetingAgenda VPA
			LEFT OUTER JOIN [VPEMeetingAgendaNotes] VPN WITH(NOLOCK)
			ON VPA.IMeetingId = VPN.IMeetingId
			AND VPA.IAgendaID = VPN.IAgendaID
			WHERE VPA.IMeetingID = @IDVal 
			AND ISNULL(Inactive,0) = 0

			SELECT @AgendaCount = COUNT(*)
								FROM #VPEMeetingAgendas
			

			SELECT @Subject = 'Meeting Minutes Added By The Supplier - ' + MeetingTitle 
							  FROM #VPEMeetingMinutesDetails N	

			SELECT DISTINCT @StrBody = '<font style="font-size:11pt;font-family:calibri;">' +  
							+ '<br>Hi,<br><br>Meeting minutes for the below meeting are added by supplier. </font><br>'
							+ '<font style="font-size:11pt;font-family:calibri;"><b><font style="font-size:14pt;">' + MeetingTitle +'</font></b><br>' 
									+ 'Created by ' + CreatedByUserName + '<br><br>'
									+ '<table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3" width="95%">'
									+ '<tr><td width="10%"><b>Meeting Date</b></td><td width="30%">' + MeetingDate + ' ' +
									+ MeetingStartTime + ' - ' + MeetingEndTime
									+ '</td><td><b>Room</b></td><td>' + Location  +'</td></tr>'
									+ '<tr><td valign="top"><b>Supplier</b></td><td>' + Supplier + '</td>'
									+ '<td><b>EGC Division</b></td><td>' + + CompanyName  +', '+ CustomerGroup  +', '+ Division
									+ '</td></tr>'
									+ '<tr><td><b>Headed By</b></td><td>' + HeadedByUserName +'</td>'
									+ '<td><b>Participants</b></td><td>' + + InternalParticipantsName + ',' + ExternalParticipantsName  + '</td></tr>'
									+ '<tr><td><b>Absentees</b></td><td colspan="3">' + AbsenteesName +'</td></tr></table><br>'
			FROM #VPEMeetingMinutesDetails
	
			SET @VPEAgendaTable='<b><font style="font-size:13pt;font-family:calibri;">Agendas</font></b><br>' + '<table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3" width="90%"><tr><td width="20%"><b>Title</b></td><td width="30%"><b>Agenda Comments</b></td><td width="10%"><b>Status</b></td></tr>' 
			SELECT @VPEAgendaTable +='<tr><td>' + AgendaTitle +'</td><td>' + Notes + '</td><td>' + AgendaStatus + '</td></tr>'
			FROM #VPEMeetingAgendas 

			SET @VPEAgendaTable += '</table>'

			SET @PageURL ='/redirect.aspx?frommode=VPE&ID='

			SELECT @StrBody = @StrBody + CASE WHEN @AgendaCount > 0 THEN @VPEAgendaTable  ELSE ''  END  + ''
							
							+'<br><a href="' + @siteUrl + @PageURL + CONVERT(NVARCHAR(100),@IDVal) +'">View Meeting Minutes</a><br><br>Thanks<br>EGC Team</font>'
			FROM #VPEMeetingMinutesDetails	

			DROP TABLE #VPEMeetingDetails
			DROP TABLE #VPEMeetingTasks
			DROP TABLE #VPEAgendas

			SET @EmailFrom ='noreply@theegc.com' -- TBA need to be discussed with Vishram and Sreenath

		END
		ELSE IF @EmailType = 323 --NMargin Review assignment
		BEGIN
			DECLARE @OrderNum NVARCHAR(100)
			DECLARE @Suf NVARCHAR(10)
			DECLARE @ProductType NVARCHAR(10)
			DECLARE @EGCPart NVARCHAR(MAX)
			DECLARE @RequestInfo NVARCHAR(100)
			DECLARE @ActionID INT
			DECLARE @Istscd INT

			SET @PageURL = '/Landing.aspx?pg=M&sessionID=psmi'
			SET @siteUrl = 'https://mrstaging.theegc.com'

			SELECT @EmailUserName =  AssignedToUsrNm,@EmailFrom = LastUpdatedByUserEmail
			,@EmailTo = AssignedToUsrEml,@replyto =  LastUpdatedByUserEmail ,@EmailBCC = LastUpdatedByUserEmail,@CONO = CONO,@Division = Division, @CustomerGrp = CustGrp
			,@OrderNum = OrderNum,@Suf= Ordersuf,@ProductType = ProductType,@CreatedByUserName = LastUpdatedByUserName,@EGCPart = EGCPart,
			@ActionID =ActionID,@Istscd = IStsCd
			FROM PABatchDetail
			WHERE ID = @IDVal	

				IF @ProductType = 'Invoice'
				BEGIN
					SET @RequestInfo = 'Invoice #'
				END
				ELSE IF @ProductType = 'Order'
				BEGIN
					SET @RequestInfo = 'Order #'
				END
				ELSE
				BEGIN
					SET @RequestInfo = 'EGC Product #'
				END

					DELETE FROM @TEMPMarginInfo
					INSERT INTO @TEMPMarginInfo
					SELECT PA.CONO AS CONO,MC.DCompanyName AS CompanyName,MC.DivDesc AS Division,MC.DCustGrpName AS CustGrp,PA.OrderNum AS OrderNo,
					PA.Ordersuf AS Suf,PA.EGCPart AS EGCPart,PA.ProductType AS ProductType,PA.Comment AS Comment
					FROM PABatchDetail PA WITH(NOLOCK)		
					INNER JOIN EGCAPPSDWHS.dbo.[DBW-SV02-DivisionDescription] MC WITH(NOLOCK)
					ON PA.CONO = MC.CONO AND PA.CustGrp  = MC.DCustGroupID AND PA.Division = MC.Div
					WHERE PA.ID = @IDVal

				IF @Istscd = 1 AND @ActionID = 3
				BEGIN
					SET @Subject = CASE 
					WHEN @ProductType = 'Invoice' OR @ProductType = 'Order' THEN 
					  'Margin Review ' + @RequestInfo + @OrderNum + '-' + ISNULL(@Suf,'0') + ' is reassigned to you by ' + @CreatedByUserName
					ELSE 
					  'Margin Review ' + @RequestInfo + ' ' + @EGCPart + ' is reassigned to you by ' + @CreatedByUserName 
					END

					IF @ProductType <> 'Product'
						BEGIN
							SET @StrBody = '<font style="font-size:11pt;font-family:calibri;">PS- this is an unattended email ID, please do not reply.<br><br>Hi ' + @EmailUserName + ',<br><br>Following Request for '+ @RequestInfo + ' ' + @OrderNum + '-'+ ISNULL(@Suf,'0') + ' is reassigned to you by '+ @CreatedByUserName + '.'										
						END
					ELSE
						BEGIN
							SET @StrBody = '<font style="font-size:11pt;font-family:calibri;">PS- this is an unattended email ID, please do not reply.<br><br>Hi ' + @EmailUserName + ',<br><br>Following Request for '+ @RequestInfo + ' ' + @EGCPart + ' is reassigned to you by '+ @CreatedByUserName + '.'										
						END	
				END
				ELSE 
				BEGIN
					SET @Subject = CASE 
					WHEN @ProductType = 'Invoice' OR @ProductType = 'Order' THEN 
					  'Margin Review ' + @RequestInfo + @OrderNum + '-' + ISNULL(@Suf,'0') + ' is Assigned to you by ' + @CreatedByUserName
					ELSE 
					  'Margin Review ' + @RequestInfo + ' ' + @EGCPart + ' is Assigned to you by ' + @CreatedByUserName 
					END

					IF @ProductType <> 'Product'
						BEGIN
							SET @StrBody = '<font style="font-size:11pt;font-family:calibri;">PS- this is an unattended email ID, please do not reply.<br><br>Hi ' + @EmailUserName + ',<br><br>Following Request for '+ @RequestInfo + ' ' + @OrderNum + '-'+ ISNULL(@Suf,'0') + ' is assigned to you by '+ @CreatedByUserName + '.'										
						END
					ELSE
						BEGIN
							SET @StrBody = '<font style="font-size:11pt;font-family:calibri;">PS- this is an unattended email ID, please do not reply.<br><br>Hi ' + @EmailUserName + ',<br><br>Following Request for '+ @RequestInfo + ' ' + @EGCPart + ' is assigned to you by '+ @CreatedByUserName + '.'										
						END
				END

				SET @xmlMarginInfo = ''
				SET @bodyMarginInfo = ''			

				SET @xmlMarginInfo = CAST(( SELECT CONO AS 'td','',CompanyName AS 'td','',CustGrp AS 'td','',Division AS 'td','',EGCPart AS 'td','',Comment AS 'td'
				FROM @TEMPMarginInfo  
				FOR XML PATH('tr'), ELEMENTS ) AS NVARCHAR(MAX))

				

				SET @bodyMarginInfo = '<br><br><table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3" width="95%">' +
						'<tr><th> CONO </th><th> CompanyName </th><th>Customer Group</th><th> Division </th><th> EGC Part# </th><th> Comments </th></tr>';
					
				SET @StrBody = @StrBody +  @bodyMarginInfo + @xmlMarginInfo + '</table>'

				SET @StrBody = @StrBody +'<br>Please visit Margin App using the link below to review the request.'
				+'<br><br><a href="' + @siteUrl+ @PageURL + '">Open Request</a><br><br>Thanks<br>EGC Team</font>'
				

		END
		ELSE IF @EmailType IN (72,73,74,75)--Expense Emails
		BEGIN
		
		  IF OBJECT_ID('tempdb..#tempExpense') IS NOT NULL
			BEGIN
				-- Drop the temporary table if it exists
				DROP TABLE #tempExpense
			END

			IF OBJECT_ID('tempdb..#tempExpenseHistory') IS NOT NULL
			BEGIN
				-- Drop the temporary table if it exists
				DROP TABLE #tempExpenseHistory
			END
		
			SELECT A.crtdunm,assignedto,sub AS [subject],total AS [Total Amount],A.updtdunm,
			expensedt,expensetype,B.curr,lineamt,B.curr+' '+CAST(lineamt AS VARCHAR(20)) AS AMT,comments,B.divnm 
			INTO #tempExpense
			FROM ExpenseHeader A
			LEFT OUTER JOIN ExpenseLine B ON A.expenseno=B.expenseno and B.isactive=1
			WHERE A.expenseno=@IDVal

			SELECT * INTO #tempExpenseHistory FROM ExpenseStatusHistory WHERE expenseno=@IDVal

			DECLARE @CreatorNM Varchar(20),@UpdtNm Varchar(20),@EXPVal Varchar(50),@CreatedDt Varchar(20),@ApprovrHistory VARCHAR(MAX),@ApprByName Varchar(20),@TotalAmount Varchar(100),@ApproveId VARCHAR(100),@ApproveQuery VARCHAR(500), @RejectQuery VARCHAR(500)
			
			SELECT @CreatorNM  = ApprByNm from #tempExpenseHistory WHERE SrNo=1
			SELECT Top 1 @UpdtNm= updtdunm From #tempExpense
			SELECT @CreatedDt  = CONVERT(NVARCHAR(30), CreatedOn, 101) + ' ' + CONVERT(NVARCHAR(30), CreatedOn, 108) + ' ' + 
			RIGHT(CONVERT(NVARCHAR(30), CreatedOn, 9), 2) 
			FROM #tempExpenseHistory WHere SrNo=1

			SET @ApprByName  = (select ApprByNm from #tempExpenseHistory A WHERE SrNo=(SELECT MAX(SrNo) FROM #tempExpenseHistory B WHERE IsCompleted=1 AND SrNo<>1))
			SET @ApproveId  = (select ApprById from #tempExpenseHistory A WHERE SrNo=(SELECT MAX(SrNo) FROM #tempExpenseHistory B WHERE IsCompleted=0 AND IsCurrentStep=1))
			SET @TotalAmount =(SELECT TOP 1 [Total Amount] FROM #tempExpense)
			SET @EXPVal='EXP'+Cast(@IDVal AS Varchar(20))
			SET @ApproveQuery ='https://expensestaging.theegc.com/Landing.aspx?pg=ApprEmail&expno=' +  CONVERT(varchar(100),@IDVal) +'&sessionID='+@ApproveId
			SET @RejectQuery = 'https://expensestaging.theegc.com/Landing.aspx?pg=RejectEmail&expno=' + CONVERT(varchar(100),@IDVal)+'&sessionID='+ @ApproveId 
 
			IF @EmailType=72 
			BEGIN												 
				SELECT DISTINCT TOP 1 @StrBody = '<font style="font-size:11pt;font-family:calibri;">Hello,<br><br>'+@CreatorNM+' submitted an expense on '+@CreatedDt+'  and it is awaiting your approval. '+IIF(@ApprByName IS NULL,'','Below are the details of expense. It''s approved by '+@ApprByName)+  '.' +
				'<br><br>Subject:' + N.subject + '<br><br> Expense Details:<br><br><table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3">
				<tr><td><b>Date</b></td><td><b>Expense Type</b></td><td>' +
				'<b>Mileage/Amount</b></td><td><b>Comments</b></td><td><b>Division</b></td></tr>'	
				FROM #tempExpense	N							
				
				SET @LineTable=''
				SELECT @LineTable = @LineTable + '<tr><td valign="top">' + CONVERT(NVARCHAR(30), N.expensedt, 101) + ' ' + CONVERT(NVARCHAR(30), N.expensedt, 108) + ' ' + 
				RIGHT(CONVERT(NVARCHAR(30), N.expensedt, 9), 2)    
				+'</td><td valign="top">'+ N.expensetype  
				+'</td><td valign="top">'+ N.AMT  
				+'</td><td valign="top">'+ N.comments  
				+'</td><td valign="top">'+ N.divnm  
				+'</td></tr>'
				FROM #tempExpense N

				SET @ApprovrHistory='<table style="font-size:11pt;font-family:calibri;" border="0" cellspacing="0" cellpadding="3"><tr><td><b>Approver History :</b></td></tr>'
				SELECT @ApprovrHistory=@ApprovrHistory+'<tr><td valign="top">' + '[ '+ApprByNm +' at ' +CONVERT(NVARCHAR(30), CreatedOn, 101) + ' ' + CONVERT(NVARCHAR(30), CreatedOn, 108) + ' ' + 
				RIGHT(CONVERT(NVARCHAR(30), CreatedOn, 9), 2)+']'+'</td></tr> </table>'  from #tempExpenseHistory
				WHERE SrNo<>1 and IsCompleted=1 ORDER by SrNo
			
				SET @StrBody = @StrBody + @LineTable  +'</table><br>Total Amount : '+@TotalAmount
				+'<br><br>'+@ApprovrHistory+'<br></font> <a  href ='+@ApproveQuery+' style=" background-color: #04AA6D; /* Green */border: none;color: white;padding: 20px;text-align: center;text-decoration: none;display: inline-block;font-size: 16px;margin: 4px 2px;cursor: pointer;border-radius: 50%;"> Approve</a>
				<a  href ='+@RejectQuery+' style=" background-color: #e60000; /* Green */border: none;color: white;padding: 20px;text-align: center;text-decoration: none;display: inline-block;font-size: 16px;margin: 4px 2px;cursor: pointer;border-radius: 50%;"> Reject </a>'
			
			END 
			ELSE IF @EmailType=73
			BEGIN
				SELECT DISTINCT TOP 1 @StrBody = '<font style="font-size:11pt;font-family:calibri;">Hello,<br><br>'+@CreatorNM+' submitted an expense on '+@CreatedDt+'  and it is awaiting your approval. Below are the details of expense.  '  +
				'<br><br>Subject:' + N.subject + '<br><br> Expense Details:<br><br><table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3">
				<tr><td><b>Date</b></td><td><b>Expense Type</b></td><td>' +
				'<b>Mileage/Amount</b></td><td><b>Comments</b></td><td><b>Division</b></td></tr>'	
				FROM #tempExpense	N							
				
				SET @LineTable=''
				SELECT @LineTable = @LineTable + '<tr><td valign="top">' + CONVERT(NVARCHAR(30), N.expensedt, 101) + ' ' + CONVERT(NVARCHAR(30), N.expensedt, 108) + ' ' + 
				RIGHT(CONVERT(NVARCHAR(30), N.expensedt, 9), 2)    
				+'</td><td valign="top">'+ N.expensetype  
				+'</td><td valign="top">'+ N.AMT  
				+'</td><td valign="top">'+ N.comments  
				+'</td><td valign="top">'+ N.divnm  
				+'</td></tr>'
				FROM #tempExpense N

				SET @ApprovrHistory='<table style="font-size:11pt;font-family:calibri;" border="0" cellspacing="0" cellpadding="3"><tr><td><b>Approver History :</b></td></tr>'
				SELECT @ApprovrHistory=@ApprovrHistory+'<tr><td valign="top">' + '[ '+ApprByNm +' at ' +CONVERT(NVARCHAR(30), CreatedOn, 101) + ' ' + CONVERT(NVARCHAR(30), CreatedOn, 108) + ' ' + 
				RIGHT(CONVERT(NVARCHAR(30), CreatedOn, 9), 2)+']'+'</td></tr> </table>'  from #tempExpenseHistory
				WHERE SrNo<>1 and IsCompleted=1 ORDER by SrNo
			
				SET @StrBody = @StrBody + @LineTable  +'</table><br>Total Amount : '+@TotalAmount
				+'<br><br>'+@ApprovrHistory+'<br></font><a  href ='+@ApproveQuery+' style=" background-color: #04AA6D; /* Green */border: none;color: white;padding: 20px;text-align: center;text-decoration: none;display: inline-block;font-size: 16px;margin: 4px 2px;cursor: pointer;border-radius: 50%;"> Approve</a>
				<a  href ='+@RejectQuery+' style=" background-color: #e60000; /* Green */border: none;color: white;padding: 20px;text-align: center;text-decoration: none;display: inline-block;font-size: 16px;margin: 4px 2px;cursor: pointer;border-radius: 50%;"> Reject </a>'
			
			END
			ELSE IF @EmailType=74
			BEGIN
				SELECT DISTINCT TOP 1 @StrBody ='<font style="font-size:11pt;font-family:calibri;">Hello,'+@CreatorNM+
				'<br><br>Expense Request '+@EXPVal+' submitted by you is approved by '  +@UpdtNm+'<br><br>Thank you,'+'<br></font>'
				FROM #tempExpense	N
			END			
			ELSE IF @EmailType=75
			BEGIN
				SELECT DISTINCT TOP 1 @StrBody ='<font style="font-size:11pt;font-family:calibri;">Hello,'+@CreatorNM+
				'<br><br>Expense Request ID'+@EXPVal+' submitted by you was rejected by '  +@UpdtNm+'<br><br>Thank you,'+'<br></font>'
				FROM #tempExpense	N
			END			
			
		END
		ELSE IF @EmailType = 77 --ACD Submitted For Approval
		BEGIN
			SET @CrtUNm =''			
			SET @Calendarquery =''	
			SET @PageURL = '/redirect.aspx?frommode=ACD&ID='

			SELECT @EmailUserName = LastUpdatedByUserName,@CrtUNm = CreatedByUserName,@ACDType=ReqType,@EmailFrom = 'vsawant@psmicorp.com',
			@replyto =  LastUpdatedByUserEmail,@EmailBCC = LastUpdatedByUserEmail,
			@CONO = CONO,@Division = Division, @CustomerGrp = CustGrp, @IStatusCode = IStatusCode			
			FROM ACDRequest WITH(NOLOCK)
			WHERE IRequestNo = @IDVal	

			DELETE FROM @TEMPACDInfo
					INSERT INTO @TEMPACDInfo
					SELECT DISTINCT APA.ACDID,MC.DCompanyName,MC.DivDesc,APA.EGCPartNo,APA.ManufacturerName,APA.ManufacturerPartNo,ISNULL(REPLACE(CPD.CustPartNo,';;',';'),'') AS CustPartNo,''
					FROM ACDRequestInfo AR WITH(NOLOCK)
					INNER JOIN ACDPartAttributes APA WITH(NOLOCK)
					ON AR.IACDID = APA.ACDID AND APA.PartType = 1
					INNER JOIN EGCAPPSDWHS.dbo.[DBW-SV02-DivisionDescription] MC WITH(NOLOCK)
					ON AR.CONO = MC.CONO AND AR.CustGrp  = MC.DCustGroupID AND AR.Division = MC.Div
					LEFT OUTER JOIN VW_ACDCustomerPartDetailsALL CPD WITH(NOLOCK)
					ON APA.ACDID = CPD.ACDID
					WHERE AR.IRequestNo = @IDVal

			DECLARE @CreatedBy NVARCHAR(100), @DivName NVARCHAR(100)
			SELECT @CreatedBy = CreatedByNm, @DivName = DivisionName FROM [PSMI-SQLSTAGE\EXTAPPS].[EGCAPPSDWHSEXT].[dbo].[ProductEnquiryRequests] ER  WHERE ER.ACDID IN (SELECT ACDID FROM @TEMPACDInfo)

		
			--SET @EmailTo ='egcappcritical@megasysindia.com;';
			SET @Subject = 'EGC ACD Request #' +CONVERT(NVARCHAR(100),@IDVal) + ' is created to change the min/max for product from customer portal'	

			SET @StrBody = '<font style="font-size:11pt;font-family:calibri;">PS- this is an unattended email ID, please do not reply.<br><br>Hi,<br><br>'+@CreatedBy+' from '+@DivName+' has raised a request to change the inventory. <br>Below ACD request has been created to change the min/max for product.'
		
			SET @xmlACDInfo = ''
			SET @bodyACDInfo = ''
		
			
				SET @xmlACDInfo = CAST(( SELECT ACDID AS 'td','',Company AS 'td','',Division AS 'td','',EGCPartNo AS 'td','',CustPartNo AS 'td','',ManufacturerName AS 'td','',ManufacturerPartNo AS 'td'
				FROM @TEMPACDInfo ORDER BY ACDID DESC 
				FOR XML PATH('tr'), ELEMENTS ) AS NVARCHAR(MAX))

				SET @bodyACDInfo = '<br><br><table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3" width="95%">' +
						'<tr><th> ACDID </th><th> Company </th><th> Division </th><th> EGC Part# </th><th> Customer Part# </th><th> MFG </th><th> MFG Part# </th></tr>';

			SET @StrBody = @StrBody +  @bodyACDInfo + @xmlACDInfo + '</table>'

			SET @StrBody = @StrBody +'<br>Please visit ACD using the link below to review the request.'
			+'<br><br><a href="' + @siteUrl+ @PageURL + CONVERT(NVARCHAR(100),@IDVal) + '&Gt=me&Type=' + CONVERT(NVARCHAR(10),@ACDType) + '&CONO=' + CONVERT(NVARCHAR(10),@CONO) + '&Division=' + CONVERT(NVARCHAR(10),@Division) +'&CustGrp=' + CONVERT(NVARCHAR(10),@CustomerGrp) + '">Open Request</a><br><br>Thanks<br>EGC Team</font>'
		
		
		END
		ELSE IF @EmailType = 78 --ACD Approved
		BEGIN
			SET @CrtUNm =''			
			SET @Calendarquery =''	
			SET @PageURL = '/redirect.aspx?frommode=ACD&ID='

			SELECT @EmailUserName = LastUpdatedByUserName,@CrtUNm = CreatedByUserName,@ACDType=ReqType,@EmailFrom = 'vsawant@psmicorp.com',
			@replyto =  LastUpdatedByUserEmail,@EmailBCC = LastUpdatedByUserEmail,
			@CONO = CONO,@Division = Division, @CustomerGrp = CustGrp, @IStatusCode = IStatusCode			
			FROM ACDRequest WITH(NOLOCK)
			WHERE IRequestNo = @IDVal	

			DELETE FROM @TEMPACDInfo
					INSERT INTO @TEMPACDInfo
					SELECT DISTINCT APA.ACDID,MC.DCompanyName,MC.DivDesc,APA.EGCPartNo,APA.ManufacturerName,APA.ManufacturerPartNo,ISNULL(REPLACE(CPD.CustPartNo,';;',';'),'') AS CustPartNo,''
					FROM ACDRequestInfo AR WITH(NOLOCK)
					INNER JOIN ACDPartAttributes APA WITH(NOLOCK)
					ON AR.IACDID = APA.ACDID AND APA.PartType = 1
					INNER JOIN EGCAPPSDWHS.dbo.[DBW-SV02-DivisionDescription] MC WITH(NOLOCK)
					ON AR.CONO = MC.CONO AND AR.CustGrp  = MC.DCustGroupID AND AR.Division = MC.Div
					LEFT OUTER JOIN VW_ACDCustomerPartDetailsALL CPD WITH(NOLOCK)
					ON APA.ACDID = CPD.ACDID
					WHERE AR.IRequestNo = @IDVal

		
			--SET @EmailTo ='egcappcritical@megasysindia.com;';
			SET @Subject = 'EGC ACD Request #' +CONVERT(NVARCHAR(100),@IDVal) + ' to change the min/max for product is completed'	

			SET @StrBody = '<font style="font-size:11pt;font-family:calibri;">PS- this is an unattended email ID, please do not reply.<br><br>Hi,<br><br>Following Item Setup/Update request to change the min/max for product is completed.'
		
			SET @xmlACDInfo = ''
			SET @bodyACDInfo = ''
		
			
				SET @xmlACDInfo = CAST(( SELECT ACDID AS 'td','',Company AS 'td','',Division AS 'td','',EGCPartNo AS 'td','',CustPartNo AS 'td','',ManufacturerName AS 'td','',ManufacturerPartNo AS 'td'
				FROM @TEMPACDInfo ORDER BY ACDID DESC 
				FOR XML PATH('tr'), ELEMENTS ) AS NVARCHAR(MAX))

				SET @bodyACDInfo = '<br><br><table style="font-size:11pt;font-family:calibri;" border="1" cellspacing="0" cellpadding="3" width="95%">' +
						'<tr><th> ACDID </th><th> Company </th><th> Division </th><th> EGC Part# </th><th> Customer Part# </th><th> MFG </th><th> MFG Part# </th></tr>';

			SET @StrBody = @StrBody +  @bodyACDInfo + @xmlACDInfo + '</table>'

			SET @StrBody = @StrBody +'<br>Please visit ACD using the link below to review the request.'
			+'<br><br><a href="' + @siteUrl+ @PageURL + CONVERT(NVARCHAR(100),@IDVal) + '&Gt=me&Type=' + CONVERT(NVARCHAR(10),@ACDType) + '&CONO=' + CONVERT(NVARCHAR(10),@CONO) + '&Division=' + CONVERT(NVARCHAR(10),@Division) +'&CustGrp=' + CONVERT(NVARCHAR(10),@CustomerGrp) + '">Open Request</a><br><br>Thanks<br>EGC Team</font>'
		
		
		END
		ELSE BEGIN
			SET @EmailTo = @EmailTo
			SET @EmailBCC = @EmailBCC
			SET @Subject =  @Subject			
			SET @StrBody = @EmailBody
			SET @Calendarquery = @Calendarquery
			SET @EmailAttachment = @EmailAttachment
			SET @Subject =  @Subject
			SET @Importance = @Importance
			SET @StrBody = @EmailBody
			SET @EmailFrom = @EmailFrom
			SET @replyto = @replyto	
		END


		IF CHARINDEX(@EmailFrom, @EmailBCC) = 0 AND  @EmailType NOT IN(11,16)
		BEGIN
			SET @EmailBCC = @EmailBCC + ';' + @EmailFrom + ';nrao@psmicorp.com;'
		END	
  
		SET @StrBody = CASE WHEN @EmailType NOT IN (101,201) THEN @BodyHeader + @StrBody + @BodyFooter ELSE @StrBody END
		
		IF @EmailType IN (48,50,7,22,23,24,25,35,36,37,54,55,57,62)
		BEGIN
			SET @EmailBCC = '';
			SET @EmailTo = 'dmargaj@psmicorp.com';
			SET @EmailCC = '';
		END

		--overwrite emails
		--SET @EmailTo = 'egcappsupportstage@megasysindia.com;'
		--SET @EmailBCC = ISNULL(@EmailBCC,'')  + ';egcappsupportstage@megasysindia.com;'

        --Expense emails
		IF @EmailType IN (72,73,74,75)
		BEGIN
			SET @EmailBCC = '';
			SET @EmailTo = 'dmargaj@psmicorp.com;rrajgopal@psmicorp.com';
			SET @EmailCC = 'dmargaj@psmicorp.com;vsawant@psmicorp.com';
		END
		IF @EmailType IN (323)
		BEGIN
			SET @EmailBCC = '';
			SET @EmailTo = 'jmadhusoodhanan@psmicorp.com;testuser2@psmicorp.com';
			SET @EmailCC = 'dmargaj@psmicorp.com';
		END

		SET @Subject = 'UAT - ' + CAST(@EmailType AS VARCHAR(20)) + @Subject
		SET @EmailAttachment= CASE WHEN @EmailType IN (101,201) THEN ISNULL(@EmailAttachment,'') ELSE CASE ISNULL(@EmailAttachment,'')  WHEN '' THEN '' ELSE ISNULL(@EmailAttachment,'') + ';' END
		+ 'C:\EGCApps\egclogo.png' END

		SET @EmailAttachment= CASE WHEN @EmailType IN (65,66,67,68,69,70,71) THEN 'C:\EGCApps\egclogo.png' ELSE  @EmailAttachment END

		IF @EmailTo <> '' AND ISNULL(@StrBody,'')<>''
		BEGIN
			print 'Email To and   email body'
		---- Printing CalendarQuery
		-- print @Calendarquery

			-- code to attach calendar only if calendar query is not blank
			IF @Calendarquery <> '' 
			BEGIN
				IF @EmailType IN (72,73,74,75)
				BEGIN
						print 'sending calendarquery email'
						
						EXEC msdb.dbo.sp_send_dbmail				
						 @recipients = 'dmargaj@psmicorp.com;rrajgopal@psmicorp.com;' --@EmailTo
						,@copy_recipients= 'rmudhe@psmicorp.com'
						,@blind_copy_recipients = ''
						,@body =@StrBody 
						,@subject = @Subject
						,@profile_name = 'EGCAppsMail'
						,@file_attachments = @EmailAttachment
						,@from_address =@EmailFrom
						,@reply_to =@replyto
						,@body_format='html'
						,@query = @Calendarquery
						,@attach_query_result_as_file = 1
						,@query_result_header = 0
						,@query_result_separator = 'CHAR(10)+CHAR(13)'
						,@query_result_width =5000
						,@exclude_query_output = 1
						,@query_attachment_filename = 'CalendarInvite.ics'			
						,@importance  = @Importance
				END				
				ELSE BEGIN

						print 'sending calendarquery email'

						EXEC msdb.dbo.sp_send_dbmail				
						 @recipients = 'sbharati@psmicorp.com;vsankar@psmicorp.com;apadmanabhan@psmicorp.com;vshah@psmicorp.com;dmargaj@psmicorp.com;pnathan@psmicorp.com;rrajgopal@psmicorp.com;' --@EmailTo
						,@copy_recipients= 'eantony@psmicorp.com;pnathan@psmicorp.com;'
						,@blind_copy_recipients = ''
						,@body =@StrBody 
						,@subject = @Subject
						,@profile_name = 'EGCAppsMail'
						,@file_attachments = @EmailAttachment
						,@from_address =@EmailFrom
						,@reply_to =@replyto
						,@body_format='html'
						,@query = @Calendarquery
						,@attach_query_result_as_file = 1
						,@query_result_header = 0
						,@query_result_separator = 'CHAR(10)+CHAR(13)'
						,@query_result_width =5000
						,@exclude_query_output = 1
						,@query_attachment_filename = 'CalendarInvite.ics'			
						,@importance  = @Importance
				END
			END
			ELSE BEGIN	
				IF @EmailType IN (77)
				BEGIN
					print 'sending  no calendarquery email'
					EXEC msdb.dbo.sp_send_dbmail				
					 @recipients = 'vsawant@psmicorp.com;vsankar@psmicorp.com;' --@EmailTo
					,@copy_recipients= 'eantony@psmicorp.com;pnathan@psmicorp.com;'
					,@blind_copy_recipients = ''
					,@body =@StrBody 
					,@subject = @Subject
					,@profile_name = 'EGCAppsMail'
					,@file_attachments = @EmailAttachment
					,@from_address =@EmailFrom
					,@reply_to =@ReplyTo
					,@body_format='html'
					,@importance  = @Importance
				END
				ELSE BEGIN
					print 'sending  no calendarquery email'
					EXEC msdb.dbo.sp_send_dbmail				
					 @recipients = 'sbharati@psmicorp.com;vsawant@psmicorp.com;apadmanabhan@psmicorp.com;vshah@psmicorp.com;dmargaj@psmicorp.com;jmadhusoodhanan@psmicorp.com;vsankar@psmicorp.com;' --@EmailTo
					,@copy_recipients= 'eantony@psmicorp.com;pnathan@psmicorp.com;'
					,@blind_copy_recipients = ''
					,@body =@StrBody 
					,@subject = @Subject
					,@profile_name = 'EGCAppsMail'
					,@file_attachments = @EmailAttachment
					,@from_address =@EmailFrom
					,@reply_to =@ReplyTo
					,@body_format='html'
					,@importance  = @Importance
				END
			END	
	  
  
		END
		
		--SELECT @EmailTo,@EmailCC,@EmailBCC,@EmailFrom,@EmailAttachment,@Subject,@StrBody,@ReplyTo
		IF @EmailType NOT IN (45,46,47,48,49,50,51,52,53,58,59,61,64) 
		BEGIN

		UPDATE EmailNotificationLog
		SET ISProcessed=1,
		ProcessedOnDatetime = GETDATE(),
		EmailSubject = @Subject,
		EmailBody = @StrBody ,
		EmailCC = @EmailCC,
		EmailBCC = @EmailBCC,
		ReplyTo = @ReplyTo,
		EmailTo = @EmailTo,
		FromAddress = @EmailFrom,
		EmailAttachment = @EmailAttachment
		WHERE IEmailSrNo=@SrNo
		END
		ELSE
		BEGIN
			UPDATE EmailNotificationLog
			SET ISProcessed=1
			WHERE IEmailSrNo=@SrNo
		END

		IF @EmailType IN (1,2,3,4,5,6)
		BEGIN
			DROP TABLE #NCDetails
		END

		FETCH NEXT FROM Cursor_ProcessEmails  
		INTO @SrNo,@EmailType,@EmailTo,@EmailCC,@EmailBCC,@IDVal,@EmailAttachment,@Subject,@EmailBody,@RecipientType,@IParticipantID,@EmailFrom
	END  
	   
		Close Cursor_ProcessEmails  
		DEALLOCATE Cursor_ProcessEmails
		
END 

/*

update EmailNotificationLog set isprocessed = 0 where  IEmailSrNo in (451792,451791,451790,451789,451788,451787,451786)

EXEC EGC_ProcessEmails 'http://egcredirect.psmicorp.com/', 'https://vpestaging.psmicorp.com/','https://meetingstaging.psmicorp.com/'
																										 


*/


