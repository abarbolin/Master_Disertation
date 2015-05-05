var serviceUrl = "http://localhost:9855/Main/InsertUserInfo";
var getIpUrl="https://jsonip.appspot.com";


var browserName = navigator.userAgent.toLowerCase();
console.log("browser name ="+ browserName);

var currentUrl = window.location.href;
console.log("currentUrl="+currentUrl);

var currentHost = location.hostname;
console.log("currentHost="+currentHost);

$.getJSON(getIpUrl,
    function(data){
       console.log("Your ip: " + data.ip);
	   
	   $.ajax({
	    method: "POST",
	    url:serviceUrl,
	    data:{
		   Url:currentUrl, 
		   Id:  data.ip,
		   BrowserType:browserName,
		   HostUrl :currentHost
	     }
        });
    });

