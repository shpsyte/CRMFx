$('ul.dropdown-menu [data-toggle=dropdown]').on('click', function (event) {
    // Avoid following the href location when clicking
    event.preventDefault();
    // Avoid having the menu to close when clicking
    event.stopPropagation();
    // If a menu is already open we close it
    //$('ul.dropdown-menu [data-toggle=dropdown]').parent().removeClass('open');
    // opening the one you clicked on
    $(this).parent().addClass('open');

    var menu = $(this).parent().find("ul");
    var menupos = menu.offset();

    if ((menupos.left + menu.width()) + 30 > $(window).width()) {
        var newpos = -menu.width();
    } else {
        var newpos = $(this).parent().width();
    }
    menu.css({ left: newpos });

});


function StringOfLink(Key1, Key2, Key3, Key4, Controller, Custom, Type) {
    var chaves = "";
    if (Key1 != "") {
        chaves = "/{3}";
    }

    if (Key2 != "") {
        chaves += "/{4}";
    }

    if (Key3 != "") {
        chaves += "/{5}";
    }

    if (Key4 != "") {
        chaves += "/{6}";
    }

    var icon = "fa fa-trash-o";
    var deletar_cancela = "Excluir";

    if (Custom == "Cancela") {
        icon = "glyphicon glyphicon-remove-sign";
        deletar_cancela = "Cancelar";
    }


    var _stringOfLinkBtnGroup = "";





    var _stringOfLinkBtnGroup = "<div class='btn-group'>" +
                    " <button type='button' class='btn btn-default dropdown-toggle' data-toggle='dropdown'> <i class='fa fa-cogs'></i> " +
                    " <span class='caret'></span> " +
                    "</button> " +
                    " <ul class='dropdown-menu'> " +
                    "  <li> <a title='Detalhar o Registro' href='{0}" + chaves + "'> <i class='fa fa-folder-open-o'></i> Detalhar </a> </li> " +
                    "  <li> <a title='Editar   o Registro' href='{1}" + chaves + "'> <i class='fa fa-pencil-square-o'></i> Editar </a> </li> " +
                    "  <li> <a title='Excluir o Registro' href='{2}" + chaves + "'> <i class='" + icon + "'></i>  " + deletar_cancela + "</a> </li> " +
                    "</ul>" +
                    "</div>"

    if (Custom == "UserDepartamento") {
        var _stringOfLinkBtnGroup = "<div class='btn-group'>" +
                        " <button type='button' class='btn btn-default dropdown-toggle' data-toggle='dropdown'> <i class='fa fa-cogs'></i> " +
                        " <span class='caret'></span> " +
                        "</button> " +
                        " <ul class='dropdown-menu'> " +
                        "  <li> <a title='Detalhar o Registro' href='{0}" + chaves + "'> <i class='fa fa-folder-open-o'></i> Detalhar </a> </li> " +
                        "  <li> <a title='Editar   o Registro' href='{1}" + chaves + "'> <i class='fa fa-pencil-square-o'></i> Editar </a> </li> " +
                        "  <li> <a title='Excluir o Registro' href='{2}" + chaves + "'> <i class='" + icon + "'></i>  " + deletar_cancela + "</a> </li> " +
                        "  <li> <a title='Usuários' href='/Procedimento/DepartamentoUsuario?cd_departamento=" + Key1 + "'> <i class='fa fa-users'></i> Usuários </a> </li> " +
                        "</ul>" +
                        "</div>"

    }


    if (Custom == "Usuario") {
        var _stringOfLinkBtnGroup = "<div class='btn-group'>" +
                        " <button type='button' class='btn btn-default dropdown-toggle' data-toggle='dropdown'> <i class='fa fa-cogs'></i> " +
                        " <span class='caret'></span> " +
                        "</button> " +
                        " <ul class='dropdown-menu'> " +
                        "  <li> <a title='Detalhar o Registro' href='{0}" + chaves + "'> <i class='fa fa-folder-open-o'></i> Detalhar </a> </li> " +
                        "  <li> <a title='Editar   o Registro' href='{1}" + chaves + "'> <i class='fa fa-pencil-square-o'></i> Editar </a> </li> " +
                        "  <li> <a title='Regional' href='/Procedimento/UsuarioRegional?cd_usuario=" + Key1 + "'> <i class='fa fa-tags'></i> Regional do Usuário </a> </li> " +
                        "  <li> <a title='Alterar Senha' href='/Usuario/Reset/" + Key1 + "'> <i class='fa fa-key'></i> Alterar a Senha </a> </li> " +
                        "</ul>" +
                        "</div>"

    }


    if (Custom == "GUsuario") {
        var _stringOfLinkBtnGroup = "<div class='btn-group'>" +
                      " <button type='button' class='btn btn-default dropdown-toggle' data-toggle='dropdown'> <i class='fa fa-cogs'></i> " +
                      " <span class='caret'></span> " +
                      "</button> " +
                      " <ul class='dropdown-menu'> " +
                      "  <li> <a title='Detalhar o Registro' href='{0}" + chaves + "'> <i class='fa fa-folder-open-o'></i> Detalhar </a> </li> " +
                      "  <li> <a title='Editar   o Registro' href='{1}" + chaves + "'> <i class='fa fa-pencil-square-o'></i> Editar </a> </li> " +
                      "  <li> <a title='Excluir o Registro' href='{2}" + chaves + "'> <i class='" + icon + "'></i>  " + deletar_cancela + "</a> </li> " +
                      "  <li> <a title='Permissões' href='/GUsuario/Permissao/?cd_grupo=" + Key1 + "'> <i class='fa fa-key'></i> Permissões </a> </li> " +
                      "</ul>" +
                      "</div>"

    }


    

    if (Custom == "ProcedimentoAdm") {
        var _stringOfLinkBtnGroup = "<a class='btn btn-info' title='Detalhar o Registro' href='{0}" + chaves + "'>  Detalhar </a> " 
    }

    if (Custom == "ProcedimentoDoc") {
        var _stringOfLinkBtnGroup = "<a class='btn btn-info btbincluirdocumento' data-id='" + chaves.replace("/","") + "' title='Alterar o Registro' href='#'>  Incluir Documento </a> "
    }



    var actionDetail = Controller + "/Details";
    var actionEdit = Controller + "/Edit";
    var actionDelete = Controller + "/Delete";
    var actionuser = "DepartamentoUsuario";


    var html = kendo.format(_stringOfLinkBtnGroup, actionDetail, actionEdit, actionDelete, Key1, Key2, Key3, Key4);
    return html;
}


function StrechString(string, inicial, final, literal) {
    var html = string.substring(inicial, final);
    var resto = string.substring(final, final + 20);

    if (resto != '')
        if (literal != '') {
            html += literal;
        }


    return html;
}

function Iniciando() {

    $('#Modalbegin').modal({
        keyboard: false,
        backdrop: 'static'
    })
    $('#Modalbegin').modal('show');


}

function Sucesso(action, controller, data, status, xhr) {
    //$('#Modalbegin').modal('hide');
    //$('#ModalError').modal('hide');

    //$('#ModalSucesso').modal({
    //    keyboard: false,
    //    backdrop: 'static'
    //})
    //$('#ModalSucesso').modal('show');


    if (action != '') {
        window.location = "/" + controller + "/" + action;
    } else {
        window.location = "/" + controller;
    }
}



function Erro() {
    $('#Modalbegin').modal('hide')
    $('#ModalError').modal({
        keyboard: false,
        backdrop: 'static'
    })
    $('#ModalError').modal('show')

}


function sleep(milliseconds) {
    var start = new Date().getTime();
    for (var i = 0; i < 1e7; i++) {
        if ((new Date().getTime() - start) > milliseconds) {
            break;
        }
    }
}





