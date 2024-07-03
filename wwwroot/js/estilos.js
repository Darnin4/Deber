$(document).ready(function () {
    $('.nav-link').hover(
        function () {
            $(this).css('color', 'turquoise'); // Cambia el color del texto a turquesa al pasar el mouse sobre el enlace
        },
        function () {
            $(this).css('color', ''); // Restaura el color original del texto al quitar el mouse del enlace
        }
    );
});
