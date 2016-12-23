function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}

       
function backToResults() {
    window.history.back();
}
        

        
function HideFormMessage() {
    $('html, body').animate({
        scrollTop: $("#responseMsgs")
    }, 1000);
    setTimeout(function () {
        $(".alert-success").hide(500, "linear");
    }, 5000);
}

function EmptyFormMessage() {
    $("#formMessages").html("");
    $(".alert-success").show();
}
        
    