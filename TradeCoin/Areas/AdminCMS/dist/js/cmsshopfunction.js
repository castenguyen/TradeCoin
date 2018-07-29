function IsEmail(email) {
    var re = /\S+@\S+\.\S+/;
    return re.test(email);
}
function IsURL(url) {
    var urlregex = new RegExp(/^HTTP|HTTP|http(s)?:\/\/(www\.)?[A-Za-z0-9]+([\-\.]{1}[A-Za-z0-9]+)*\.[A-Za-z]{2,40}(:[0-9]{1,40})?(\/.*)?$/);
    return urlregex.test(url);
}
function ValidateEmail($email) {
    var re = /\S+@\S+\.\S+/;
    return re.test($email);

}
function CheckSpecialCharacterString(str) {
    var re = /^[a-zA-Z0-9- ]*$/;
    return re.test(str);
}


function ToFriendlyUrl(str) {
    str = str.toLowerCase();
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    str = str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'| |\"|\&|\#|\[|\]|~|$|_/g, "-");

    str = str.replace(/-+-/g, "-"); //thay thế 2- thành 1-
    str = str.replace(/^\-+|\-+$/g, "");
    var lastChar = str[str.length - 1];
    if (lastChar == '-') {
        str = str.substring(0, str.length - 1);
    }
    return str;
}

function ValidationForm(allData)
{
    $.validator.setDefaults({
        ignore: []
    });
    var ctrID = allData.ctrID;
    var ctrID2 = "#" + ctrID;
    var rules = allData.rules;
    var messages = allData.messages;

    var vd = $(ctrID2).validate({
        rules: rules,
        messages: messages
    });
}

function Product(data) {
    this.ProductId = data.ProductId;
    this.ParentId = data.ParentId;
    this.ProductCD = data.ProductCD;

    this.ViewCount = data.ViewCount;
    this.LikeCount = data.LikeCount;
    this.CommentCount = data.CommentCount;

    this.ProductName = data.ProductName;
    this.FriendlyURL = data.ProductId;
    this.OldPrice = data.OldPrice;
    this.NewPrice = data.NewPrice;
    this.SKUCode = data.SKUCode;
}

function AddProductToPromotion(ProductId,ProductName) {
    $("#ParentblockPlusProduct").append('<div class="blockPlusProduct_'
        + ProductId + '"><input name="PromotionProductId" type="hidden" value="'
        + ProductId   + '"><h4>'
        + ProductName + ' </h4><a href="javascript:RemoveProductToPromotion('
        + ProductId   + ');">Xoá</a><br></div>');
}

function RemoveProductToPromotion(ProductId) {
    $('.blockPlusProduct_' + ProductId).remove();
}






