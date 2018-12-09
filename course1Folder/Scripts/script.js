/*$(document).ready(function () {
    $(document).on("click",function () {
        $("main").toggle();
    });
});*/
/*$(document).ready(function(){
    //при нажатию на любую кнопку, имеющую класс .btn
    $(".PostWrapper").click(function() {
        //открыть модальное окно с id="myModal"
        $("#myModal").modal('show');
    });
});*/
var modalWasntOpened = true;
$(document).ready(function(){
    $('.slider').slick({
        nextArrow: $(".slideRight"),
        prevArrow: $(".slideLeft"),
        speed: 10
    });
});

$(document).ready(function () {
    $(".PostWrapper").on("click",function () {
        $("#myModal").modal('show');
    });
});

$(document).ready(function(){
    $('#myModal').on('shown.bs.modal', function() {
        if(modalWasntOpened){
            $(".slideLeft").click();
            $(".slideRight").click();
        }
        modalWasntOpened=false;
    });
});

