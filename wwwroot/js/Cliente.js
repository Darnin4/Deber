$(document).ready(function () {
    $('#cliente-select').change(function () {
        var cedula = $(this).val();
        if (cedula) {
            $.ajax({
                url: '/Cliente/GetCliente?cedula=' + cedula,
                type: 'GET',
                success: function (cliente) {
                    $('#nombre-cliente').val(cliente.Nombre);
                    $('#apellido-cliente').val(cliente.Apellido);
                    $('#direccion-cliente').val(cliente.Direccion);
                    $('#telefono-cliente').val(cliente.Telefono);
                },
                error: function () {
                    alert('Se  encontro el cliente.');
                }
            });
        } else {
            $('#cliente-info input').val('');
        }
    });


    // Obtener datos del cliente al seleccionar uno

    document.getElementById('cliente-select').addEventListener('change', function () {
        var clienteId = this.value;
        if (clienteId) {
            fetch('/Cliente/ObtenerDatosCliente?clienteId=' + clienteId)
                .then(response => response.json())
                .then(data => {
                    document.getElementById('nombre-cliente').value = data.nombres;
                    document.getElementById('apellido-cliente').value = data.apellidos;
                    document.getElementById('direccion-cliente').value = data.direccion;
                    document.getElementById('telefono-cliente').value = data.telefono;
                })
                .catch(error => {
                    console.error('Error al obtener los datos del cliente:', error);
                    alert('Error al cargar los datos del cliente.');
                });
        } else {
            document.getElementById('nombre-cliente').value = '';
            document.getElementById('apellido-cliente').value = '';
            document.getElementById('direccion-cliente').value = '';
            document.getElementById('telefono-cliente').value = '';
        }
    });












    $('#agregar-producto').click(function () {
        var productoId = $('#producto-select').val();
        if (productoId) {
            $.ajax({
                url: '/Producto/GetProducto/' + productoId,
                type: 'GET',
                success: function (producto) {
                    var newRow = `<tr>
                        <td>${producto.Nombre}</td>
                        <td><input class="form-control cantidad-producto" type="number" value="1" /></td>
                        <td>${producto.Precio}</td>
                        <td class="total-producto">${producto.Precio}</td>
                        <td><button class="btn btn-danger eliminar-producto">Eliminar</button></td>
                    </tr>`;
                    $('#productos-seleccionados').append(newRow);
                    actualizarTotales();
                }
            });
        }
    });

    $(document).on('change', '.cantidad-producto', function () {
        var row = $(this).closest('tr');
        var precio = parseFloat(row.find('td').eq(2).text());
        var cantidad = $(this).val();
        var total = precio * cantidad;
        row.find('.total-producto').text(total);
        actualizarTotales();
    });

    // Función para eliminar una fila de la tabla
    $(document).on('click', '.btnEliminarFila', function () {
        $(this).closest('tr').remove();
        calcularTotales();
    });

    function actualizarTotales() {
        var subTotal = 0;
        $('#productos-seleccionados tr').each(function () {
            var totalProducto = parseFloat($(this).find('.total-producto').text());
            subTotal += totalProducto;
        });
        var iva = subTotal * 0.12;
        var total = subTotal + iva;
        $('#sub-total').val(subTotal.toFixed(2));
        $('#iva').val(iva.toFixed(2));
        $('#total').val(total.toFixed(2));
    }
});






////PRUEBA//////////
    var index = 0;

    // Función para agregar un producto a la tabla
    document.getElementById('agregar-producto').addEventListener('click', function () {
            var productoId = document.getElementById('producto-select').value;
    var cantidad = parseFloat(document.getElementById('Cantidad').value);
    var precio = parseFloat(document.getElementById('Precio').value);
    var descuento = parseFloat(document.getElementById('Descuento').value);

    if (productoId && !isNaN(cantidad) && !isNaN(precio) && !isNaN(descuento)) {
                var total = cantidad * precio - descuento;
    var nombreProducto = $('#producto-select option:selected').text();
    var fila = `<tr>
        <td>${nombreProducto}</td>
        <td><input type="number" class="form-control cantidad" name="DetalleFactura[${index}].Cantidad" value="${cantidad}" readonly /></td>
        <td><input type="number" class="form-control precio" name="DetalleFactura[${index}].Precio" value="${precio}" readonly /></td>
        <td><input type="number" class="form-control descuento" name="DetalleFactura[${index}].Descuento" value="${descuento}" readonly /></td>
        <td><input type="text" class="form-control total" name="DetalleFactura[${index}].Total" value="${total.toFixed(2)}" readonly /></td>
        <td><button type="button" class="btn btn-danger btn-sm btnEliminarFila">Eliminar</button></td>
    </tr>`;
                document.getElementById('productos-seleccionados').insertAdjacentHTML('beforeend', fila);
                index++;
                calcularTotales();
            } else {
                alert('Debe seleccionar un producto y completar los campos de cantidad, precio y descuento.');
            }
        });

        // Función para calcular totales
        function calcularTotales() {
            var subTotal = 0;
            $('.total').each(function () {
                subTotal += parseFloat($(this).val());
            });
            $('#sub-total').val(subTotal.toFixed(2));

            // Ejemplo de cálculo de IVA y Total
            var iva = subTotal * 0.12; // Suponiendo un IVA del 12%
            $('#iva').val(iva.toFixed(2));

            var total = subTotal + iva;
            $('#total').val(total.toFixed(2));
}



   
