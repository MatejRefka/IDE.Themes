//toggle navitem background color based on current page
var loc = window.location.pathname;

$(".navitems").find("a").each(function () {
    $(this).toggleClass("active", $(this).attr("href") == loc);
});


