$().ready(function () {

    $('#login').click(function () {
        //alert('in login');
        var msg = ''; 
        
        var div = $('#errorDiv');

       
        if (($('#username').val() == '' || $('#username').val() == null || $('#username').val() == "undefined") && ($('#passwrd').val() == '' || $('#passwrd').val() == null)) {
          
            msg = 'Please enter username and password';
            var errorDiv = '<div class="col-md-12 col-sm-12" style="color:red;padding-left: 30% !important;">' + msg + '</div>';
            div.empty();
            div.append(errorDiv);

            return false;
        }
        else {

            if ($('#username').val() == '' || $('#username').val() == null) {
               
                msg = 'Please enter username ';
                var errorDiv = '<div class="col-md-12 col-sm-12" style="color:red;padding-left: 30% !important;">' + msg + '</div>';
                div.empty();
                div.append(errorDiv);

                return false;

            }

           

            if ($('#passwrd').val() == '' || $('#passwrd').val() == null) {
               
                msg = 'Please enter password';
                var errorDiv = '<div class="col-md-12 col-sm-12" style="color:red;padding-left: 30% !important;">' + msg + '</div>';
                div.empty();
                div.append(errorDiv);

                return false;
            }
        }

       

        return true;
    });

});