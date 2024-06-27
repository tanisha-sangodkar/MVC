<!-- image slider or caruosel code -->

	let slides = document.getElementsByClassName('slide');
	let img_num = 0;
	slideshow(img_num);

	var prev_button = document.getElementById('prev-button');
	var next_button = document.getElementById('next-button');

	var num_images = slides.length - 1;

	prev_button.addEventListener('click', function () {
		img_num = img_num - 1;
		if (img_num < 0) {
			img_num = num_images;
			slideshow(img_num);
		}
		else {
			slideshow(img_num);
		}

	})

	next_button.addEventListener('click', function () {
		img_num = img_num + 1;
		if (img_num > num_images) {
			img_num = 0;
			slideshow(img_num);
		}
		else {
			slideshow(img_num);
		}

	})

	function slideshow(number) {
		for (i = 0; i <= num_images; i++) {
			slides[i].style.display = "none";
		}
		slides[number].style.display = "block";

	}

