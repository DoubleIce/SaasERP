/** 
*  ����JS ͨ���� 
*  author:panfu 
*/  
  
  
/** 
* ȥ��ǰ��ո� 
* " dd ".trim(); == "dd" 
*/  
String.prototype.trim = function() {  
    return this.replace(/(^\s*)|(\s*$)/g, "");  
}  
  
/** 
* ȥ����ո� 
* " dd".leftTrim(); == "dd" 
*/  
String.prototype.leftTrim = function() {  
    return this.replace(/(^\s*)/g, "");  
}  
  
/** 
* ȥ���ҿո� 
* "dd ".rightTrim(); == "dd" 
*/  
String.prototype.rightTrim = function() {  
    return this.replace(/(\s*$)/g, "");   
}  
/** 
* ֻ��������(0123456789) 
* "dd 09".toNumber(); == "" 
* onkeyup="change_number(this)"  
* onafterpaste="change_number(this)" 
*/  
String.prototype.toNumber = function() {  
    return this.replace(/\D/g, "");  
  
  
/** 
* ɾ������ָ���±��ָ������ 
* arr.remove(2);//ɾ���±�Ϊ2�Ķ��󣨴�0��ʼ���㣩 
* arr.remove(str);//ɾ��ָ������ 
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
* ��ʱ��ת���ɹ̶���ʽ��� 
* new Date().toFormat('yyyy-MM-dd HH:mm:ss'); 
* new Date().toFormat('yyyy/MM/dd hh:mm:ss'); 
* ֻ֧�ֹؼ��֣�yyyy��MM��dd��HH��hh��mm��ss��HH����ʾ24Сʱ��hh��ʾ12Сʱ 
*/  
Date.prototype.toFormatString=function(format){  
    var formatstr = format;  
    if(format != null && format != ""){  
        //������  
        if(formatstr.indexOf("yyyy") >=0 ){  
            formatstr = formatstr.replace("yyyy",this.getFullYear());  
        }  
        //������  
        if(formatstr.indexOf("MM") >=0 ){  
            var month = this.getMonth() + 1;  
            if(month < 10){  
                month = "0" + month;  
            }  
            formatstr = formatstr.replace("MM",month);  
        }  
        //������  
        if(formatstr.indexOf("dd") >=0 ){  
            var day = this.getDay();  
            if(day < 10){  
                day = "0" + day;  
            }  
            formatstr = formatstr.replace("dd",day);  
        }  
        //����ʱ - 24Сʱ  
        var hours = this.getHours();  
        if(formatstr.indexOf("HH") >=0 ){  
            if(month < 10){  
                month = "0" + month;  
            }  
            formatstr = formatstr.replace("HH",hours);  
        }  
        //����ʱ - 12Сʱ  
        if(formatstr.indexOf("hh") >=0 ){  
            if(hours > 12){  
                hours = hours - 12;  
            }  
            if(hours < 10){  
                hours = "0" + hours;  
            }  
            formatstr = formatstr.replace("hh",hours);  
        }  
        //���÷�  
        if(formatstr.indexOf("mm") >=0 ){  
            var minute = this.getMinutes();  
            if(minute < 10){  
                minute = "0" + minute;  
            }  
            formatstr = formatstr.replace("mm",minute);  
        }  
        //������  
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
  
//�뿪��ҳ��ʱ����ʾ��  
window.onbeforeunload = function() {  
    if (commn.IsSearch == true) {  
        return "\n���棡~  \n��������ִ���У�ȷ����Ҫ������\n";  
    }  
}  
  
//commn����  
var commn ={  
    IsSearch:false,//�Ƿ����ڲ�ѯ����  
    InputDisabled: function(eid) {//��ť����󣬰�ť������ ���磺window.setTimeout("commn.InputDisabled('#bt_submit,#bt_back')", 1);  
        commn.IsSearch =true;  
        jQuery(eid).attr("disabled","disabled");  
    },  
    DateDiffDay:function (beginDate,endDate){//��ȡ����ʱ���������  
        //beginDate��endDate ��ʽ��2011-8-25  
        var arrDate = new Array();  
        //���ÿ�ʼʱ��  
        arrDate = beginDate.split("-");  
        beginDate = new Date(arrDate[1] + "/" + arrDate[2] + "/" + arrDate[0]);//Ĭ�ϸ�ʽ��8/25/2011  
        //���ý���ʱ��  
        arrDate = endDate.split("-");  
        endDate = new Date(arrDate[1] + "/" + arrDate[2] + "/" + arrDate[0]);//Ĭ�ϸ�ʽ��8/25/2011  
        var iDays = parseInt(Math.abs((beginDate-endDate)/1000/60/60/24));//ת���죬Ĭ�Ϻ���  
        return iDays;  
    },  
    DateTimeIsFomart:function (val){//��֤ʱ�����ʽ�Ƿ���ȷ12:00:25  
        //�ж�ʱ��λ���Ƿ���ȷ  
        if(val.length == 8){  
            var val_r = val.replace(/\D/g,'');//ֻȡ����  
            if(val_r.length == 6){//�ж�λ���Ƿ���ȷ  
                var val_s = val.split(":");//�����ֳ�����  
                if(val_s.length == 3){//���������ȷ  
                    var v0 = parseInt(val_s[0]);  
                    var v1 = parseInt(val_s[1]);  
                    var v2 = parseInt(val_s[2]);  
                    // ��ʱ�����ֵ ����������Χʱ������true  
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
* ����jquery-1.3.2.min.js 
*/ 
document.write("<script language='javascript' src='js/jquery-1.3.2.min.js'></script>"); 
/* 
* �������� 
*/ 
var hostUrl='http://'+window.location.host; //��ȡ��վ����ͷ 
/* 
* ˮƽ����leftֵ 
*/ 
function HorCenter(x){ 
return (document.documentElement.clientWidth-x)/2; 
} 
/* 
* ��ֱ����topֵ 
*/ 
function VerCenter(y){ 
return (document.documentElement.clientHeight-y)/2+document.documentElement.scrollTop; 
} 
/* 
* ɾ���������˵Ŀո� 
*/ 
function Trim(str){ 
return str.replace(/(^\s*)|(\s*$)/g, ""); 
} 
/* 
* �ж����䣬����true/false 
*/ 
function IsEmail(email){ 
var Expression=/\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w([-.]\w+)*/; 
var objExp=new RegExp(Expression); 
return objExp.test(email); 
} 
/* 
* �ж��û���������true/false 
*/ 
function IsUser(user){ 
var Expression=/^(?!_)(?!.*?_$)(\w|[\u4E00-\u9FA5])*$/; //ֻ�����֡����ġ���ĸ���»�����ϣ��»��߲����ڿ�ͷ���β 
var objExp=new RegExp(Expression); 
return objExp.test(user); 
} 
/* 
* �ж��ֻ����� 
*/ 
function IsMobile(mobile){ 
var Expression=/^1[3458]{1}[0-9]{9}$/; 
var objExp=new RegExp(Expression); 
return objExp.test(mobile); 
} 
/* 
* �жϷǸ�����������true/false 
*/ 
function IsInt(intval){ 
var Expression=/^\d+$/; 
var objExp=new RegExp(Expression); 
return objExp.test(intval); 
} 
/* 
* �ж����֣�����true/false 
*/ 
function IsNum(num){ 
return !isNaN(num); 
} 
/* 
* �ж�����ң�Ǯ��������true/false 
*/ 
function IsMoney(money){ 
var Expression=/^(([1-9]\d+|0)\.\d{2}|([1-9]\d+|0))$/; 
var objExp=new RegExp(Expression); 
return objExp.test(money); 
} 
/* 
* �����ַ������ȣ����ļ�Ϊ������Ӣ�����ּ�Ϊһ�� 
*/ 
function GetByteLen(sChars){ 
return sChars.replace(/[^\x00-\xff]/g,"xx").length; 
} 
/* 
* �����������ַ��� 
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
* д��cookie 
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
* ��ȡcookie 
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
* checkBoxȫѡ��ȫ�� 
* 
* ���� 
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
* ֧�ֶ���������ĸ��� 
*/ 
function CopyValue(strValue){ 
if(IsIE()) 
{ 
clipboardData.setData("Text",strValue); 
alert("�ɹ�����"); 
} 
else 
{ 
Copy(strValue); 
alert("�ɹ�����"); 
} 
} 
/* 
* �ж�IE����� 
*/ 
function IsIE(number){ 
if(typeof(number)!=number) 
{ 
return!!document.all; 
} 
} 

//��һ��������jQuery��
//<script type="text/javascript" src="<%=path%>/resources/js/jquery.min.js"></script>
 


/*****************************************************************
                  jQuery Ajax��װͨ����  (linjq)       
*****************************************************************/
$(function(){
    /**
     * ajax��װ
     * url ��������ĵ�ַ
     * data ���͵������������ݣ�����洢���磺{"date": new Date().getTime(), "state": 1}
     * async Ĭ��ֵ: true��Ĭ�������£����������Ϊ�첽���������Ҫ����ͬ�������뽫��ѡ������Ϊ false��
     *       ע�⣬ͬ��������ס��������û�������������ȴ�������ɲſ���ִ�С�
     * type ����ʽ("POST" �� "GET")�� Ĭ��Ϊ "GET"
     * dataType Ԥ�ڷ��������ص��������ͣ����õ��磺xml��html��json��text
     * successfn �ɹ��ص�����
     * errorfn ʧ�ܻص�����
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
     * ajax��װ
     * url ��������ĵ�ַ
     * data ���͵������������ݣ�����洢���磺{"date": new Date().getTime(), "state": 1}
     * successfn �ɹ��ص�����
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
     * ajax��װ
     * url ��������ĵ�ַ
     * data ���͵������������ݣ�����洢���磺{"date": new Date().getTime(), "state": 1}
     * dataType Ԥ�ڷ��������ص��������ͣ����õ��磺xml��html��json��text
     * successfn �ɹ��ص�����
     * errorfn ʧ�ܻص�����
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

 