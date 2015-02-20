$(".menu-item")
    .click(function (item) {
        $(item)
            .addClass("active")
            .find("content")
            .fadeIn("fast")
            .css("margin-right", "10px");
    })