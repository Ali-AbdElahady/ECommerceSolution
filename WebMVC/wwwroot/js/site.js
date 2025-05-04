// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


// cart

jQuery(document).ready(function () {
    const cart = JSON.parse(localStorage.getItem("cart") || "[]");
    console.log("cart item")
    //console.log($(".remove-btn"))
    //console.log("audhasjlhdoai's;hfjlaks")
    if (cart.length === 0) {
        $('#cart-content').html("<p>Your cart is empty.</p>");
        return;
    }

    $.ajax({
        url: '/Client/Cart/Load',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(cart),
        success: function (html) {
            $('#cart-content').html(html);
        },
        error: function () {
            $('#cart-content').html("<p>Failed to load cart.</p>");
        }
    });
});

