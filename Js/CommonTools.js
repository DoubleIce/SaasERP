Trim = function (str) {
    return null == str ? str : str.replace(/^\s+|\s+$/g, "");
}

CreateXMLDOM = function () {
    try {
        this.xmlDoc = new ActiveXObject("MSXML2.DOMDocument");
        return this.xmlDoc;
    }
    catch (e) {
        alert("DOM document not created. Check MSXML version used in createXmlDomDocument.");
        return null;
    }
}

// 切除方法 
String.prototype.trim = function (value) {
    if (value) {
        var exp = eval("/^" + value + "+|" + value + "+$/g");
        return this.replace(exp, "");
    }
    return this.replace(/^\s+|\s+$/g, "");
}

//名称：日期格式转换
//参数：日期字符串和格式类型
//type： 0--用'-'分隔日期  1--用'/'分隔日期  2--年月日分隔日期  3--包含具体的时间信息
DateFormat = function (date, type) {
    var dateFormat = "";
    var dateSplit = date.split("T");

    switch (type) {
        case "0": dateFormat = dateSplit[0];
            break;
        case "1": dateFormat = dateSplit[0].replace(/-/g, "/");
            break;
        case "2": var arrDate = dateSplit[0].split("-");
            dateFormat = arrDate[0] + "年" + arrDate[1] + "月" + arrDate[2] + "日";
            break;
        case "3":
            if (dateSplit[1].indexOf(".") != -1) {
                dateFormat = dateSplit[0] + " " + dateSplit[1].split(".")[0];
            }
            else if (dateSplit[1].indexOf("+") != -1) {
                dateFormat = dateSplit[0] + " " + dateSplit[1].split("+")[0];
            }
            break;
    }

    return dateFormat;
}