$(document).ready(function () {

    $('#frmNewClaim').hide();
    $("#claimDetails").hide();
    $("#claimInvoices").hide();
    $("#claimUpdate").hide();

    $("#accordian h3").click(function () {
        //slide up all the link lists
        $("#accordian ul ul").slideUp();
        //slide down the link list below the h3 clicked - only if its closed
        if (!$(this).next().is(":visible")) {
            $(this).next().slideDown();
        }
    });


    $("#newClaim").click(function () {
        $("#newClaim").addClass("claimTitleActive").removeClass("claimTitleNotActive");
        $("#claimList").addClass("claimTitleNotActive").removeClass("claimTitleActive");
        
        $("#frmNewClaim").slideToggle("slow");
        $("#frmClaimSumary").slideToggle("slow");
    });

    $("#claimList").click(function () {
        $("#claimList").addClass("claimTitleActive").removeClass("claimTitleNotActive");
        $("#newClaim").addClass("claimTitleNotActive").removeClass("claimTitleActive");

        $("#frmClaimSumary").slideToggle("slow");
        $("#frmNewClaim").slideToggle("slow");
        collapseClaimData();
    });

    $(".claimInfoTitle").click(function () {
        $("#frmClaimDetails").slideToggle("slow");
    });

    $(".updateclaimTitle").click(function () {
        $("#frmUpdateClaim").slideToggle("slow");
    });

    $(".invoiceTitle").click(function () {
        $("#frmInvoice").slideToggle("slow");
    });
});

function collapse(id) {
    i
}

function openClaimData(oppname) {
    $("#claimDetails").show();
    $("#claimInvoices").show();
    $("#claimUpdate").show();
}

function collapseClaimData() {
    $("#claimDetails").hide();
    $("#claimInvoices").hide();
    $("#claimUpdate").hide();
}