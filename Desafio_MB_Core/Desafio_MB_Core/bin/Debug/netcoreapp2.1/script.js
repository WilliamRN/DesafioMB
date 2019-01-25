//in JQuery ...
//$(document).ready(function() {
//same as ...
//$(function(){
//http://localhost:57623/api/values
$(function(){
	/*
	$("#hidden").hover(function() {
		$(this).css("color","black");
	},
	function() {
		$(this).css("color","white");
	});
	*/
	$("button").click(function() {
		//alert("incoming");
		$.get("http://localhost:57623/api/values", function(data, status){
			//$("#hidden").text("Data:" + data);
			alert("Data:" + data + "\nStatus:" + status);
		});
	});
})