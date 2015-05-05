 $(document).ready(function(){
	 var getIpUrl="https://jsonip.appspot.com";
	 function GetUserLogin() {
    try {
        var GetLoginUrl = "http://localhost:9855/Main/GetUserLogin";
		
        		 
		 var login = $.cookie('login');
		 if(login === undefined)
		 {
			 $.getJSON(getIpUrl, function(data) {
                $.ajax({
                    method: "GET",
                    url: GetLoginUrl,
                    data: {
                        ip: data.ip
                    },
                    success: function(response) {
                        login = response;
						$.cookie("login", login, { expires: 7});
                        setLoginToInput(login);
                    }

                });

            }); 
		 }
		 else{
			 setLoginToInput(login);
		 }
           
    } catch (err) {
        alert(err);
    }


}
	 //Вызываем функцию загрузки логина
	 GetUserLogin();
	 //Устанавливаем логин в поле для ввода
	 function setLoginToInput(login)
	 {
		$("#loginTxt").val(login); 
	 }
	 
  
  $('#btn_submit').click(function(e) {

                try {
                    e.preventDefault();

					
                    var serviceUrl = "http://localhost:9855/Main/SetLogin";

                    var input = $('#loginTxt').val();


                    $.getJSON(getIpUrl,
                        function(data) {

                            $.ajax({
                                method: "POST",
                                url: serviceUrl,
                                data: {
                                    login: input,
                                    ip: data.ip
                                },
								success:function(response) {
									$.cookie("login", input, { expires: 7});
									setLoginToInput(input);
								}
                            });
                        });
				}
                    catch (err) {
                        alert(err);
                    }
					
                });
  
		
		
    }); 
	