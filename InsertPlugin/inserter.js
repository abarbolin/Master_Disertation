var getIpUrl="https://jsonip.appspot.com";

var currentUrl = window.location.href;
console.log("currentUrl="+currentUrl);

var userIp = 0;
$.getJSON(getIpUrl,
    function(data){
	   userIp = data.ip;
       console.log("Your ip: " + data.ip);
    });

/*var serviceUrl = "www.google.ru";
var userId = 5;
$.ajax{
	method: "POST",
	url:serviceUrl,
	data:{
		url:currentUrl, userId: userId
	}
	.success{
		console.log("Данные успешно отправлены");
	}
	.error{ function(response){
		console.log("Произошла ошибка :"+response);
	}		
	}
}*/