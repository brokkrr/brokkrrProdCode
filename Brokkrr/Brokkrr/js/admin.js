$().ready(function () {


    var searchby = $('#filter').val();
    var toSearch = $('#txtTofilter').val();
    var msg = '';
    var flag = 0;

    
    //if (ViewBag.userdetails == null)
    //{

    //    $('#noUser').css("display","block");
    //}


    $('#txtSearchby').keyup(function () {

        if ($('#txtSearchby').val() != '') {

            $('#btnSearch').css("disabled", "false");
        }
      });




    if (searchby != '' || searchby != null)
    {
        $('#selectBy').val(searchby);


        if (searchby == "FirstName") {
            $('#txtSearchby').css("display", "block");
            $('#txtSearchby').val(toSearch);
            $('#selectRole').css("display", "none");
            $('#selectStatus').css("display", "none");
            $('#spanSearch').css("display", "block");

        }

        if (searchby == "UserType") {
            $('#txtSearchby').css("display", "none");
            $('#selectRole').css("display", "block");
            $('#selectRole').val(toSearch);
            $('#selectStatus').css("display", "none");
            $('#spanSearch').css("display", "block");

        }

        if (searchby == "IsActive") {
            $('#txtSearchby').css("display", "none");
            $('#selectRole').css("display", "none");
            $('#selectStatus').css("display", "block");
            $('#selectStatus').val(toSearch);

            $('#spanSearch').css("display", "block");

        }




    }


    



    



    $('#selectBy').change(function () {

        var selectBy = $('#selectBy :selected').val();

        $('#filter').val(selectBy);

        if ($('#selectBy :selected').val() =="FirstName") {
            $('#txtSearchby').css("display", "block");
            $('#selectRole').css("display", "none");
            $('#selectStatus').css("display", "none");
            $('#spanSearch').css("display", "block");
            msg = 'Please enter name';
            flag = 1;
            $('#error').empty();
            
        }

        if ($('#selectBy :selected').val() == "UserType") {
            $('#txtSearchby').css("display", "none");
            $('#selectRole').css("display", "block");
            $('#selectStatus').css("display", "none");
            $('#spanSearch').css("display", "block");
            msg = 'Please select Role';
            flag = 2;
            $('#error').empty();
           
        }

        if ($('#selectBy :selected').val() == "IsActive") {
            $('#txtSearchby').css("display", "none");
            $('#selectRole').css("display", "none");
            $('#selectStatus').css("display", "block");
            $('#spanSearch').css("display", "block");
            msg = 'Please select status';
            flag = 3;
            $('#error').empty();
        }
       


    });

    $('#selectRole').change(function () {
        
        var role = $('#selectRole :selected').val();
        if (role != "-1") {
            $('#txtTofilter').val(role);
            $('#error').empty();
        }

    });

    $('#selectStatus').change(function () {
        var status = $('#selectStatus :selected').val();
        if (status != "-1") {
            $('#txtTofilter').val(status);
            $('#error').empty();
        }

    });

    $('#txtSearchby').keyup(function () {

       
        if ($('#txtSearchby').val() != '') {

            $('#txtTofilter').val($('#txtSearchby').val());
            $('#error').empty();
        }

    });




    $('#btnSearch').click(function () {


        var errorDiv = '<div class="col-md-12 col-sm-12" style="color:red;padding-left: 30% !important;margin-top: -15px;">' + msg + '</div>';
        var div = $('#error');

        switch (flag)
        {

            case 1: if ($('#txtSearchby').val() == '' || $('#txtSearchby').val() == null) {

                div.empty();
                div.append(errorDiv);
               
                         return false;
                   }
                break;
            case 2: if ($('#selectRole :selected').val() == "-1") {

                div.empty();
                div.append(errorDiv);

                return false;
            }
                break;
            case 3: if ($('#selectStatus :selected').val() == "-1") {

                div.empty();
                div.append(errorDiv);

                return false;
            }
                break;
        }
        return true;
    });


    
});