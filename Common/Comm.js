/** 
*  常用JS 通用类 
*  author:panfu 
*/  
  
  
/** 
* 去掉前后空格 
* " dd ".trim(); == "dd" 
*/  
String.prototype.trim = function() {  
    return this.replace(/(^\s*)|(\s*$)/g, "");  
}  
  
/** 
* 去掉左空格 
* " dd".leftTrim(); == "dd" 
*/  
String.prototype.leftTrim = function() {  
    return this.replace(/(^\s*)/g, "");  
}  
  
/** 
* 去掉右空格 
* "dd ".rightTrim(); == "dd" 
*/  
String.prototype.rightTrim = function() {  
    return this.replace(/(\s*$)/g, "");   
}  
/** 
* 只留下数字(0123456789) 
* "dd 09".toNumber(); == "" 
* onkeyup="change_number(this)"  
* onafterpaste="change_number(this)" 
*/  
String.prototype.toNumber = function() {  
    return this.replace(/\D/g, "");  
  
  
/** 
* 删除数组指定下标或指定对象 
* arr.remove(2);//删除下标为2的对象（从0开始计算） 
* arr.remove(str);//删除指定对象 
*/  
Array.prototype.remove=function(obj){  
    for(var i =0;i <this.length;i++){  
        var temp = this[i];  
        if(!isNaN(obj)){  
            temp=i;  
        }  
        if(temp == obj){  
            for(var j = i;j <this.length;j++){  
                this[j]=this[j+1];  
            }  
            this.length = this.length-1;  
        }     
    }  
}  
/** 
* 将时间转换成固定格式输出 
* new Date().toFormat('yyyy-MM-dd HH:mm:ss'); 
* new Date().toFormat('yyyy/MM/dd hh:mm:ss'); 
* 只支持关键字（yyyy、MM、dd、HH、hh、mm、ss）HH：表示24小时，hh表示12小时 
*/  
Date.prototype.toFormatString=function(format){  
    var formatstr = format;  
    if(format != null && format != ""){  
        //设置年  
        if(formatstr.indexOf("yyyy") >=0 ){  
            formatstr = formatstr.replace("yyyy",this.getFullYear());  
        }  
        //设置月  
        if(formatstr.indexOf("MM") >=0 ){  
            var month = this.getMonth() + 1;  
            if(month < 10){  
                month = "0" + month;  
            }  
            formatstr = formatstr.replace("MM",month);  
        }  
        //设置日  
        if(formatstr.indexOf("dd") >=0 ){  
            var day = this.getDay();  
            if(day < 10){  
                day = "0" + day;  
            }  
            formatstr = formatstr.replace("dd",day);  
        }  
        //设置时 - 24小时  
        var hours = this.getHours();  
        if(formatstr.indexOf("HH") >=0 ){  
            if(month < 10){  
                month = "0" + month;  
            }  
            formatstr = formatstr.replace("HH",hours);  
        }  
        //设置时 - 12小时  
        if(formatstr.indexOf("hh") >=0 ){  
            if(hours > 12){  
                hours = hours - 12;  
            }  
            if(hours < 10){  
                hours = "0" + hours;  
            }  
            formatstr = formatstr.replace("hh",hours);  
        }  
        //设置分  
        if(formatstr.indexOf("mm") >=0 ){  
            var minute = this.getMinutes();  
            if(minute < 10){  
                minute = "0" + minute;  
            }  
            formatstr = formatstr.replace("mm",minute);  
        }  
        //设置秒  
        if(formatstr.indexOf("ss") >=0 ){  
            var second = this.getSeconds();  
            if(second < 10){  
                second = "0" + second;  
            }  
            formatstr = formatstr.replace("ss",second);  
        }  
    }  
    return formatstr;  
}  
  
//离开该页面时，提示！  
window.onbeforeunload = function() {  
    if (commn.IsSearch == true) {  
        return "\n警告！~  \n操作正在执行中，确认需要继续？\n";  
    }  
}  
  
//commn对象  
var commn ={  
    IsSearch:false,//是否正在查询数据  
    InputDisabled: function(eid) {//按钮点击后，按钮不可用 例如：window.setTimeout("commn.InputDisabled('#bt_submit,#bt_back')", 1);  
        commn.IsSearch =true;  
        jQuery(eid).attr("disabled","disabled");  
    },  
    DateDiffDay:function (beginDate,endDate){//获取两个时间的天数差  
        //beginDate、endDate 格式：2011-8-25  
        var arrDate = new Array();  
        //设置开始时间  
        arrDate = beginDate.split("-");  
        beginDate = new Date(arrDate[1] + "/" + arrDate[2] + "/" + arrDate[0]);//默认格式：8/25/2011  
        //设置结束时间  
        arrDate = endDate.split("-");  
        endDate = new Date(arrDate[1] + "/" + arrDate[2] + "/" + arrDate[0]);//默认格式：8/25/2011  
        var iDays = parseInt(Math.abs((beginDate-endDate)/1000/60/60/24));//转换天，默认毫秒  
        return iDays;  
    },  
    DateTimeIsFomart:function (val){//验证时分秒格式是否正确12:00:25  
        //判断时间位数是否正确  
        if(val.length == 8){  
            var val_r = val.replace(/\D/g,'');//只取数字  
            if(val_r.length == 6){//判读位置是否正确  
                var val_s = val.split(":");//按：分成数组  
                if(val_s.length == 3){//如果数组正确  
                    var v0 = parseInt(val_s[0]);  
                    var v1 = parseInt(val_s[1]);  
                    var v2 = parseInt(val_s[2]);  
                    // 当时分秒的值 处于正常范围时，返回true  
                    if(v0 != null && (v0 >= 0 && v0 <= 23) &&   
                       v1 != null && (v1 >= 0 && v1 <= 59) &&   
                       v2 != null && (v2 >= 0 && v2 <= 59)   
                    ){  
                       return true;  
                    }  
                }  
            }  
        }  
        return false;  
    }  
}  
 

/* 
* 包含jquery-1.3.2.min.js 
*/ 
document.write("<script language='javascript' src='js/jquery-1.3.2.min.js'></script>"); 
/* 
* 公共参数 
*/ 
var hostUrl='http://'+window.location.host; //获取网站主机头 
/* 
* 水平居中left值 
*/ 
function HorCenter(x){ 
return (document.documentElement.clientWidth-x)/2; 
} 
/* 
* 垂直居中top值 
*/ 
function VerCenter(y){ 
return (document.documentElement.clientHeight-y)/2+document.documentElement.scrollTop; 
} 
/* 
* 删除左右两端的空格 
*/ 
function Trim(str){ 
return str.replace(/(^\s*)|(\s*$)/g, ""); 
} 
/* 
* 判断邮箱，返回true/false 
*/ 
function IsEmail(email){ 
var Expression=/\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w([-.]\w+)*/; 
var objExp=new RegExp(Expression); 
return objExp.test(email); 
} 
/* 
* 判断用户名，返回true/false 
*/ 
function IsUser(user){ 
var Expression=/^(?!_)(?!.*?_$)(\w|[\u4E00-\u9FA5])*$/; //只能数字、中文、字母、下划线组合，下划线不能在开头或结尾 
var objExp=new RegExp(Expression); 
return objExp.test(user); 
} 
/* 
* 判断手机号码 
*/ 
function IsMobile(mobile){ 
var Expression=/^1[3458]{1}[0-9]{9}$/; 
var objExp=new RegExp(Expression); 
return objExp.test(mobile); 
} 
/* 
* 判断非负整数，返回true/false 
*/ 
function IsInt(intval){ 
var Expression=/^\d+$/; 
var objExp=new RegExp(Expression); 
return objExp.test(intval); 
} 
/* 
* 判断数字，返回true/false 
*/ 
function IsNum(num){ 
return !isNaN(num); 
} 
/* 
* 判断人民币（钱），返回true/false 
*/ 
function IsMoney(money){ 
var Expression=/^(([1-9]\d+|0)\.\d{2}|([1-9]\d+|0))$/; 
var objExp=new RegExp(Expression); 
return objExp.test(money); 
} 
/* 
* 计算字符串长度，中文记为两个，英文数字记为一个 
*/ 
function GetByteLen(sChars){ 
return sChars.replace(/[^\x00-\xff]/g,"xx").length; 
} 
/* 
* 限制输入的最长字符串 
*/ 
function GetByteVal(sSource, iLen){ 
if(sSource.replace(/[^\x00-\xff]/g,"xx").length<=iLen) 
{ 
return sSource; 
} 
else 
{ 
var str=""; 
var l=0; 
var schar; 
for(var i=0;schar=sSource.charAt(i);i++) 
{ 
str+=schar; 
l+=(schar.match(/[^\x00-\xff]/) != null ? 2:1); 
if(l>=iLen) 
{ 
break; 
} 
} 
return str; 
} 
} 
/* 
* 写入cookie 
*/ 
function SetCookie(name,value) 
{ 
var argv=SetCookie.arguments; 
var argc=SetCookie.arguments.length; 
var expires=(2<argc)?argv[2]:null; 
var path=(3<argc)?argv[3]:null; 
var domain=(4<argc)?argv[4]:null; 
var secure=(5<argc)?argv[5]:false; 
document.cookie=name+"="+escape(value)+((expires==null)?"":("; expires="+expires.toGMTString()))+((path==null)?"":("; path="+path))+((domain==null)?"":("; domain="+domain))+((secure==true)?"; secure":""); 
} 
/* 
* 获取cookie 
*/ 
function GetCookie(name){ 
var search = name + "="; 
var returnvalue = ""; 
if (document.cookie.length > 0) 
{ 
offset = document.cookie.indexOf(search); 
if (offset != -1) 
{ 
offset += search.length; 
end = document.cookie.indexOf(";", offset); 
if (end == -1) 
end = document.cookie.length; 
returnvalue=unescape(document.cookie.substring(offset,end)); 
} 
} 
return returnvalue; 
} 
/* 
* checkBox全选、全消 
* 
* 引用 
* <input name="chkbox" type="checkbox" onclick="checkAll(this,'form1')" /> 
* <input name="chkbox" type="checkbox" value="" class="chk" /> 
*/ 
function CheckAll(obj,objForm){ 
if(obj.checked==true){ 
$('#'+objForm+' input:checkbox.chk').each(function(){ 
this.checked='checked'; 
}); 
} 
else{ 
$('#'+objForm+' input:checkbox.chk').each(function(){ 
this.checked=''; 
}); 
} 
} 
/* 
* 支持多种浏览器的复制 
*/ 
function CopyValue(strValue){ 
if(IsIE()) 
{ 
clipboardData.setData("Text",strValue); 
alert("成功复制"); 
} 
else 
{ 
Copy(strValue); 
alert("成功复制"); 
} 
} 
/* 
* 判断IE浏览器 
*/ 
function IsIE(number){ 
if(typeof(number)!=number) 
{ 
return!!document.all; 
} 
} 

//第一步：引入jQuery库
//<script type="text/javascript" src="<%=path%>/resources/js/jquery.min.js"></script>
 


/*****************************************************************
                  jQuery Ajax封装通用类  (linjq)       
*****************************************************************/
$(function(){
    /**
     * ajax封装
     * url 发送请求的地址
     * data 发送到服务器的数据，数组存储，如：{"date": new Date().getTime(), "state": 1}
     * async 默认值: true。默认设置下，所有请求均为异步请求。如果需要发送同步请求，请将此选项设置为 false。
     *       注意，同步请求将锁住浏览器，用户其它操作必须等待请求完成才可以执行。
     * type 请求方式("POST" 或 "GET")， 默认为 "GET"
     * dataType 预期服务器返回的数据类型，常用的如：xml、html、json、text
     * successfn 成功回调函数
     * errorfn 失败回调函数
     */
    jQuery.ax=function(url, data, async, type, dataType, successfn, errorfn) {
        async = (async==null || async=="" || typeof(async)=="undefined")? "true" : async;
        type = (type==null || type=="" || typeof(type)=="undefined")? "post" : type;
        dataType = (dataType==null || dataType=="" || typeof(dataType)=="undefined")? "json" : dataType;
        data = (data==null || data=="" || typeof(data)=="undefined")? {"date": new Date().getTime()} : data;
        $.ajax({
            type: type,
            async: async,
            data: data,
            url: url,
            dataType: dataType,
            success: function(d){
                successfn(d);
            },
            error: function(e){
                errorfn(e);
            }
        });
    };
    
    /**
     * ajax封装
     * url 发送请求的地址
     * data 发送到服务器的数据，数组存储，如：{"date": new Date().getTime(), "state": 1}
     * successfn 成功回调函数
     */
    jQuery.axs=function(url, data, successfn) {
        data = (data==null || data=="" || typeof(data)=="undefined")? {"date": new Date().getTime()} : data;
        $.ajax({
            type: "post",
            data: data,
            url: url,
            dataType: "json",
            success: function(d){
                successfn(d);
            }
        });
    };
    
    /**
     * ajax封装
     * url 发送请求的地址
     * data 发送到服务器的数据，数组存储，如：{"date": new Date().getTime(), "state": 1}
     * dataType 预期服务器返回的数据类型，常用的如：xml、html、json、text
     * successfn 成功回调函数
     * errorfn 失败回调函数
     */
    jQuery.axse=function(url, data, successfn, errorfn) {
        data = (data==null || data=="" || typeof(data)=="undefined")? {"date": new Date().getTime()} : data;
        $.ajax({
            type: "post",
            data: data,
            url: url,
            dataType: "json",
            success: function(d){
                successfn(d);
            },
            error: function(e){
                errorfn(e);
            }
        });
    };



});



function $(id){document.getElementById(id)} 



function $() 
{ 
var elements = new Array(); 
for (var i = 0; i < arguments.length; i++) 
{ 
var element = arguments[i]; 
if (typeof element == 'string') 
element = document.getElementById(element); 
if (arguments.length == 1) 
return element; 
elements.push(element); 
} 
return elements; 



function $(objectId) { 
if(document.getElementById && document.getElementById(objectId)) { 
return document.getElementById(objectId);// W3C DOM 
} else if (document.all && document.all(objectId)) { 
return document.all(objectId);// MSIE 4 DOM 
} else if (document.layers && document.layers[objectId]) { 
return document.layers[objectId];// NN 4 DOM.. note: this won't find nested layers 
} else { 
return false; 
} 
} 



} 

 