/// <binding AfterBuild="sass, minify-js" />
"use strict";

var gulp   = require("gulp");
var sass   = require("gulp-sass");
var del    = require("del");
var concat = require("gulp-concat");
var rename = require("gulp-rename");
var uglify = require("gulp-uglify");

gulp.task("sass", function () {
	del("Assets/CSS/roadkill.css");
	del("Assets/CSS/roadkill.installer.css");

	gulp
		.src("Assets/CSS/roadkill.scss")
		.pipe(sass({ outputStyle: "compressed" }))
		.pipe(sass().on("error", sass.logError))
		.pipe(gulp.dest("Assets/CSS/"));

	gulp
		.src("Assets/CSS/roadkill.installer.scss")
		.pipe(sass({ outputStyle: "compressed" }))
		.pipe(sass().on("error", sass.logError))
		.pipe(gulp.dest("Assets/CSS/"));
});

gulp.task("minify-js", function () {
	del("Assets/Scripts/roadkill.min.js");

	var srcFiles = [
		"Assets/Scripts/jquery/*.js",
		"Assets/Scripts/roadkill/*.js",
		"Assets/Scripts/roadkill/editpage/*.js",
		"Assets/Scripts/roadkill/filemanager/*.js",
		"Assets/Scripts/roadkill/sitesettings/*.js",
		"Assets/Scripts/shared/*.js"
	];

	// The installer JS files aren"t included, they use the Typescript compiler output.
	gulp.src(srcFiles)
		.pipe(concat("roadkill.min.js"))
		.pipe(uglify())
		.pipe(gulp.dest("Assets/Scripts/"));
});