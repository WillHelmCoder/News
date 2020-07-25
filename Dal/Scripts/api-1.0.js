var tam = 0;
var url_path = "/home/noticia";
var show = 8;
var magazineId = 9;
var url = window.location.href;

var url_news = "http://expose.inovercy.com";


function data() {
    var QueryString = function () {

        var query_string = {};
        var query = window.location.search.substring(1);
        var vars = query.split("&");
        for (var i = 0; i < vars.length; i++) {
            var pair = vars[i].split("=");
            if (typeof query_string[pair[0]] === "undefined") {
                query_string[pair[0]] = pair[1];
            } else if (typeof query_string[pair[0]] === "string") {
                var arr = [query_string[pair[0]], pair[1]];
                query_string[pair[0]] = arr;
            } else {
                query_string[pair[0]].push(pair[1]);
            }
        }
        return query_string;
    }();

    var arr;
    var id = QueryString.Noticia;
    var permalink = QueryString.Noticia;
    if (!id) {
        $.ajax({
            crossDomain: true,
            type: "GET",
            contentType: "application/json; charset=utf-8",
            async: false,
            url: url_news + "/api/news/NewsByMagazine/" + magazineId,
            dataType: "json",
            success: function (data) {
                arr = data;

            }
        });
        return arr;

    } else {
        $.ajax({
            crossDomain: true,
            type: "GET",
            contentType: "application/json; charset=utf-8",
            async: false,
            url: url_news + "/api/news/" + id,
            //data: { projectID: 1 },
            dataType: "json",
            success: function (data) {
                arr = data;
            }
        });
        return arr;
    }
}//data

function Category() {
    $.ajax({
        crossDomain: true,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: false,
        url: url_news + "/api/Categories/MagazineCategories/" + magazineId,
        dataType: "json",
        success: function (data) {
            categorys = data;

        }
    });
    return categorys;
}

function Update() {

    var newInfo;
    var newTam;
    var newCat;

    newInfo = data();
    newTam = newInfo.length;
    newCat = Category();

    console.log(newInfo);
    console.log(newCat);
    console.log(newTam);

    if (newTam != tam) {
        console.log("actualizacion de informacion");
        preparar();
        Display(newInfo, newCat);
        pagination();
        sort();
        tam = newTam;
    }
}


function Display(Info, Category) {
    $('#news').hide();
    var display = '';

    if (jQuery.type(Info) == "array") {
        $.each(Info, function (index, obj) {
            var category = obj.CategoryName;
            var category = category.replace(/\s/g, '');
            var linkNoticia = "?Noticia=" + obj.NewsId + "/" + obj.Permalink;
            var img = url_news + "/Content/data/" + obj.Image;
            var titulo = obj.Title.length > 30 ? obj.Title.substring(0, 30) + "..." : obj.Title;
            var fecha = obj.Date;
            var texto = obj.Description.substring(0, 50);
            var alt = obj.Alt;
            var permalink = obj.permalink;


            display += '<div class="col-md-4 portfolio-item mix ' + category + '">';//col                                
            display += '<div class="img-container">';//img-container 
            display += '<a href="' + url_path + linkNoticia + '">';
            display += '<img class="img-responsive " src="' + img + '" alt="' + titulo + '">';
            display += '</a>';
            display += '</div>';//img-container  end
            display += '<h5>';
            display += '<a href="' + linkNoticia + '">' + titulo + '</a>';
            display += '  <small>' + fecha + '</small>';
            display += '</h5>';
            display += '<p class="">' + texto;
            display += '</p>';

            display += '</div>';//col end

        });

        $('#noticias').prepend('<ol class="breadcrumb" role="tablist" id="navbar-buttons">');

        $.each(Category, function (index, obj) {
            var category = obj.Name;
            var category = category.replace(/\s/g, '');
            $('#navbar-buttons').append('<li role="presentation" id="button-filter" class="filter navbar-btn noaction " cat="' + category + ' " data-filter=".' + category + '">' + '<a href="#" class="">' + obj.Name + '</a></li>');

        });




        $("#news").append("<div class='Sort-Container' id='Container'> </div>")
        $("#Container").append(display);
        $("#news").fadeIn(1500);
    } else {


        display += '<div class="col-md-10 col-lg-10  col-md-offset-1 col-lg-offset-1 centered">';
        display += '<div class="css-img-container">';
        display += '<img class="img-responsive" alt="' + Info.Title + '" src="' + url_news + '/Content/data/' + Info.Image + '">';
        display += '</div>';
        display += '<div class="clearfix">';
        display += '<h1>' + Info.Title + '</h1>';
        display += '<div class="css-date">';
        display += Info.Date;
        display += '</div>';
        display += '</div>';
        display += '<p class="css-bodytext">' + Info.Body + '</p>';
        display += '<a class="fa fa-facebook-official" href="http://www.facebook.com/sharer.php?u=' + url + '&t=' + Info.Title + '"><img alt="Facebook" src="http://newssystem.yoalty.com/Content/data/FACEBOOK.png"></a>'
        display += '<a class="fa fa-facebook-official" href="https://twitter.com/proyectocuente"><img alt="Twitter" src="http://newssystem.yoalty.com/Content/data/TWITTER.png"></a>'
        display += '</div>';



        $("#news").append(display);

        $('head').append('<meta name="Title" content="' + Info.Title + '">');
        $('head').append('<meta name="Description" content="' + Info.MetaDesc + '">');

        $("#news").fadeIn(0);

    }
}


function preparar() {
    var hook = $("#noticias");
    var container = $("<div>", { class: "container Paloma1" });
    var fill = $("<div>", { class: "row ", id: "news", text: "" });
    var nav = $("<div>", { class: "text-center ", id: "nav" });


    hook.html(container);
    container.append(fill);

    var display = "";
    display += "<input type='hidden' id='current_page' />";
    display += "<input type='hidden' id='show_per_page' />";
    display += "<div id='page_navigation' class='text-center'></div>";

    $("#news").after(display);
}

function Base() {
    preparar();

    Update();

}
///////////////////////////////////////////SCRIPTS INDEPENDIENTES//////////////////////////////////////////////////
$(function () {
    $('.noaction').click(function () {

        return false;
    });
});

function sort() {
    $(".filter").on('click', function () {
        var category = $(this).attr('data-filter');

        $('#Container').children().css('display', 'none');
        $(category + ".active_new").css('display', 'inline');

    });
}



/////////////////////////////////////////////////PAGINADOR////////////////////////////////////////////////////////

function pagination() {

    //how much items per page to show
    var show_per_page = show;
    //getting the amount of elements inside content div
    var number_of_items = $('#Container').children().size();

    //calculate the number of pages we are going to have
    var number_of_pages = Math.ceil(number_of_items / show_per_page);
    //set the value of our hidden input fields
    $('#current_page').val(1);
    $('#show_per_page').val(show_per_page);

    //now when we got all we need for the navigation let's make it '

    /*
	what are we going to have in the navigation?
		- link to previous page
		- links to specific pages
		- link to next page
	*/

    var navigation_html = '<ul class="pagination" >';
    // navigation_html += '<li>';
    //navigation_html += '<a class="previous_link" href="javascript:previous();">Prev</a>';
    //navigation_html += '</li>';
    var current_link = 0;
    while (number_of_pages > current_link) {
        navigation_html += '<li>';
        navigation_html += '<a class="page_link" href="javascript:go_to_page(' + current_link + ')" longdesc="' + current_link + '">' + (current_link + 1) + '</a>';
        navigation_html += '</li>';
        current_link++;
    }
    //navigation_html += '<li>';
    //navigation_html += '<a class="next_link" href="javascript:next();">Next</a>';
    //navigation_html += '</li>';
    navigation_html += '</ul>';


    $('#page_navigation').html(navigation_html);

    //add active_page class to the first page link
    $('#page_navigation .page_link:first').addClass('active_page');
    //hide all the elements inside content div
    $('#Container').children().css('display', 'none');

    //and show the first n (show_per_page) elements

    $('#Container').children().css('display', 'none').removeClass('active_new').addClass('inactive_new').slice(0, show_per_page).css('display', 'block').addClass('active_new').removeClass('inactive_new');


}

function previous() {

    new_page = parseInt($('#current_page').val()) - 1;
    //if there is an item before the current active link run the function
    if ($('.active_page').prev('.page_link').length == true) {
        go_to_page(new_page);
    }

}

function next() {
    new_page = parseInt($('#current_page').val()) + 1;
    //if there is an item after the current active link run the function
    if ($('.active_page').next('.page_link').length == true) {
        go_to_page(new_page);
    }

}
function go_to_page(page_num) {
    //get the number of items shown per page
    var show_per_page = parseInt($('#show_per_page').val());

    //get the element number where to start the slice from
    start_from = page_num * show_per_page;

    //get the element number where to end the slice
    end_on = start_from + show_per_page;

    //hide all children elements of content div, get specific items and show them
    $('#Container').children().css('display', 'none').removeClass('active_new').addClass('inactive_new').slice(start_from, end_on).css('display', 'block').addClass('active_new').removeClass('inactive_new');
    //$('#Container').children().slice(start_from, end_on).css('display', 'block').addClass('active_new');

    /*get the page link that has longdesc attribute of the current page and add active_page class to it
	and remove that class from previously active page link*/
    $('.page_link[longdesc=' + page_num + ']').addClass('active_page').siblings('.active_page').removeClass('active_page');

    //update the current page input field
    $('#current_page').val(page_num);
}


/////////////////////////////////////////////////PAGINADOR////////////////////////////////////////////////////////

/// run the script
Base();
setInterval(function () { Update(); }, 9000);














