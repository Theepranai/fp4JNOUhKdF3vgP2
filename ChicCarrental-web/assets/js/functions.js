jQuery(document).ready(function (jQuery) {
    var jQueryscrollTop;
    var jQueryheaderheight;
    var jQueryloggedin = false;

    if (jQueryloggedin == false) {
        jQueryheaderheight = jQuery('.navbar-wrapper2').height() - 20;
    } else {
        jQueryheaderheight = jQuery('.navbar-wrapper2').height() + 100;
    }


    jQuery(window).scroll(function () {
        var jQueryiw = jQuery('body').innerWidth();
        jQueryscrollTop = jQuery(window).scrollTop();
        if (jQueryiw < 992) {

        }
        else {
            jQuery('.navbar-wrapper2').css({ 'min-height': 110 - (jQueryscrollTop / 2) + 'px' });
        }
        jQuery('#dajy').css({ 'top': ((- jQueryscrollTop / 5) + jQueryheaderheight) + 'px' });
        //jQuery(".sboxpurple").css({'opacity' : 1-(jQueryscrollTop/300)});
        jQuery(".scrolleffect").css({ 'top': ((- jQueryscrollTop / 5) + jQueryheaderheight) + 50 + 'px' });
        jQuery(".tp-leftarrow").css({ 'left': 20 - (jQueryscrollTop / 2) + 'px' });
        jQuery(".tp-rightarrow").css({ 'right': 20 - (jQueryscrollTop / 2) + 'px' });
    });

});

//------------------------------
//Scroll animations
//------------------------------
jQuery(window).scroll(function () {
    var jQueryiw = jQuery('body').innerWidth();

    if (jQuery(window).scrollTop() != 0) {
        jQuery('.mtnav').stop().animate({ top: '0px' }, 500);
        jQuery('.logo').stop().animate({ width: '100px' }, 100);
    }
    else {
        if (jQueryiw < 992) {
        }
        else {
            jQuery('.mtnav').stop().animate({ top: '30px' }, 500);
        }

        jQuery('.logo').stop().animate({ width: '120px' }, 100);

    }


    //Social
    if (jQuery(window).scrollTop() >= 900) {
        jQuery('.social1').stop().animate({ top: '0px' }, 100);

        setTimeout(function () {
            jQuery('.social2').stop().animate({ top: '0px' }, 100);
        }, 100);

        setTimeout(function () {
            jQuery('.social3').stop().animate({ top: '0px' }, 100);
        }, 200);

        setTimeout(function () {
            jQuery('.social4').stop().animate({ top: '0px' }, 100);
        }, 300);

        setTimeout(function () {
            jQuery('.gotop').stop().animate({ top: '0px' }, 200);
        }, 400);

    }
    else {
        setTimeout(function () {
            jQuery('.gotop').stop().animate({ top: '100px' }, 200);
        }, 400);
        setTimeout(function () {
            jQuery('.social4').stop().animate({ top: '-120px' }, 100);
        }, 300);
        setTimeout(function () {
            jQuery('.social3').stop().animate({ top: '-120px' }, 100);
        }, 200);
        setTimeout(function () {
            jQuery('.social2').stop().animate({ top: '-120px' }, 100);
        }, 100);

        jQuery('.social1').stop().animate({ top: '-120px' }, 100);

    }


});	

// ########################
// BACK TO TOP FUNCTION
// ########################


jQuery(document).ready(function(){
"use strict";
	
	// hide #back-top first
	jQuery("#back-top").hide();
	
	// fade in #back-top
	jQuery(function () {
		jQuery(window).scroll(function () {
			if (jQuery(this).scrollTop() > 700) {
				jQuery('#back-top').fadeIn();
			} else {
				jQuery('#back-top').fadeOut();
			}
		});

		// scroll body to 0px on click
		jQuery('#back-top a').click(function () {
			jQuery('body,html').animate({
				scrollTop: 0
			}, 500);
			return false;
		});
		
				// scroll body to 0px on click
		jQuery('a#gotop2').click(function () {
			jQuery('body,html').animate({
				scrollTop: 0
			}, 500);
			return false;
		});
		
		var jQueryih = jQuery('body').innerHeight();
		
		jQuery(".scroll").click(function(event){		
			event.preventDefault();
			jQuery('html,body').animate({scrollTop:jQuery(this.hash).offset().top - jQueryih}, 1500);
		});
		
		
		
		
		
	});
});



