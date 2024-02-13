var mixinArray = [];



const Mixin = {
    methods: {
        clicked() {
            alert("WORKed!")
        },
        onCategorySearch(search, loading) {
            this.GenerateDropDowns('CATEGORY', search)
        },
        onStatusSearch(search, loading) {
            this.GenerateDropDowns('STATUS', search)
        },
        onDpCodeSearch(search, loading) {
            this.GenerateDropDowns('BRANCH', search)
        },
        onCircleSearch(search, loading) {
            this.GenerateDropDowns('CIRCLE', search)
        },
        onDesignationSearch(search, loading) {
            this.GenerateDropDowns('DESIGNATION', search)
        },
        onStateSearch(search, loading) {
            this.GenerateDropDowns('STATE', search)
        },
        onMemberSearch(search, loading) {
            this.GenerateDropDowns('MEMBER', search)
        },
        onAllMemberSearch(search, loading) {
            this.GenerateDropDowns('ALLMEMBER', search)
        },
        onAllMemberSearchSelected(search, loading, selecteditem) {
            this.GenerateDropDowns('ALLMEMBER', search, selecteditem)
        },
        GenerateDropDowns: function (listType, search,selecteditem,dependentvalue) {
            var bodyFormData = new FormData();
            bodyFormData.append('listType', listType);
            bodyFormData.append('q', search);
            bodyFormData.append('selecteditem', selecteditem);
            bodyFormData.append('dependentvalue', dependentvalue);
            axios({
                method: 'post',
                url: '/api/data/',
                data: bodyFormData,
                headers: { 'Content-Type': 'multipart/form-data' }
            })
                .then(function (response) {
                    //handle success

                    if (listType === 'BRANCH') {
                        this.dpCodes = response.data.items;
                        this.SetSelecteddpCodes();
                    }
                    else if (listType === 'CIRCLE') {
                        this.circles = response.data.items;
                        this.SetSelectedcircles();
                    }
                    else if (listType === 'STATUS') {
                        this.statuses = response.data.items;
                        this.SetSelectedstatus();
                    }
                    else if (listType === 'CATEGORY') {
                        this.categories = response.data.items;
                        this.SetSelectedcategorys();
                    }
                    else if (listType === 'DESIGNATION') {
                        this.designations = response.data.items;
                        this.SetSelecteddesignations();
                    }
                    else if (listType === 'STATE') {
                        this.states = response.data.items;
                        this.SetSelectedstates();
                    }
                     else if (listType === 'MEMBER') {
                        this.members = response.data.items;
                        this.SetSelectedMembers();
                    }
                    else if (listType === 'ALLMEMBER') {
                        this.members = response.data.items;
                        this.SetSelectedMembers();
                    }
                    
                }.bind(this))
                .catch(function (response) {
                    //handle error
                    console.log(response);
                });
        },
    }
}
mixinArray.push(Mixin);