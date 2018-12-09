function Create() {
    $.get("@Url.Action('Create')").done(function (res) {
        $("#CreateForm").html(res);
    });
}

function getFormData(selector) {
    var $form = $(selector);
    const unindexedArray = $form.find(':input').serializeArray();
    const indexedArray = {};

    $.map(unindexedArray, function (n) {
        indexedArray[n.name] = n.value.trim();
    });

    return indexedArray;
}

function Saveform() {

    var fd = getFormData(".nashaForma");


    var images = $.map($(".Img_Preview"), function (item,i) {
        return { Id:$(item).data('image-id') };
    });

    fd["Images"] = images;


    $.post("@Url.Action(\"Create\")", fd).done(function (res) {



    });

}


function UploadImage(elem) {


    var files = $(elem).prop('files');
    if (files.length) {
        var data = new FormData();
        data.append('imageData', files[0]);


        $.ajax({
            url: "@Url.Action(\"UploadImages\")",
            data: data,
            processData: false,
            contentType: false,
            type: 'POST'
        }).done(function (res) {

            if (res.Success) {
                $("#PicturesDiv").append("<img src=\"@Url.Action(\"GetImage'\")/" + res.Result + "'  class='Img_Preview'  data-image-id='" + res.Result + "'/>")


                var html = $(elem).parent().html();
                $(elem).parent().html(html);
            }

        });

    }
}


$(document).off("click", ".Img_Preview");

$(document).on("click", ".Img_Preview", function () {
    var $elem = $(this);

    var id = $elem.data("image-id");

    $.post("@Url.Action(\"DelImage\")", { id: id }).done(function (res) {
        if (res.Success) {

            $("[data-image-id='" + id + "']").remove();

        } else {
            alert(res.Result);
        }


    });
});