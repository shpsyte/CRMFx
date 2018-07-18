function initMap(EndCli, Componente) {
    //sleep(3000);
    var latlng = new google.maps.LatLng(-25.429862, -49.195388);
    var mapOptions =
    {
        zoom: 15,
        center: latlng,
        mapTypeId: google.maps.MapTypeId.TERRAIN
    }


    map = new google.maps.Map(document.getElementById(Componente), mapOptions);



    var geocoder = new google.maps.Geocoder();

    //var address = "Capitão Roberto lopes quintas 124";

    geocoder.geocode({ 'address': EndCli }, function (results, status) {
        if (status === google.maps.GeocoderStatus.OK) {
            map.setCenter(results[0].geometry.location);
            var marker = new google.maps.Marker({
                map: map,
                position: results[0].geometry.location
            });
        } else {
            map = new google.maps.Map(document.getElementById(Componente), mapOptions);


        }
    });
}



