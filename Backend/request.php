<?php
header("Content-Type: text/plain");
if(isset($_SERVER['HTTPS']) && $_SERVER['HTTPS'] === 'on')
    $link = "https";
else
    $link = "http";
$link .= "://";
$link .= $_SERVER['HTTP_HOST'];
$link .= $_SERVER['REQUEST_URI'];
$url_components = parse_url($link);
parse_str($url_components['query'], $params);
//string elaboration
if (isset($params['p']))
	file_put_contents("status.txt", $params['p']);
$response = file_get_contents("status.txt");
if (isset($params['d']))
	file_put_contents("status.txt", "");
echo $response;
?>