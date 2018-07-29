<?php
// configure here
define("YOUR_EMAIL", "info@localhost.dom");
define('EMAIL_SUBJECT', 'Message from Your Web-site');

// There is no need to change anything down below, unless you're really sure.


// Content type header
header('Content-Type: application/json');

// Message Structure
$message = <<<HEREDOC
Message from your web-site. \n
Name : %s \n 
E-mail: %s \n
Message: %s \n
\n
Kind Regards,\n
BetaDesign
HEREDOC;

// Locale String
$strings = array(
	'err_name'    => 'Please enter your name',
	'err_email'   => 'Please enter a valid email',
	'err_unknown' => 'An unexpected error occured, please try again later',
	'success'     => 'Your email has been sent.',
);


// Logic goes here, I suggest really really don't go down there.
// Filter the data
$data = filter_var_array($_POST, FILTER_SANITIZE_SPECIAL_CHARS);

try {

	// Validate the data
	if (!isset($data['name']) || strlen($data['name']) < 3) {
		throw new JSError($strings['err_name']);
	}
	if ( !isset($data['email']) || ! filter_var($data['email'], FILTER_VALIDATE_EMAIL) ) {
		throw new JSError($strings['err_email']);
	}


	// The message content
	if (!mail(YOUR_EMAIL, EMAIL_SUBJECT, sprintf($message, $data['name'], $data['email'], @$data['message'])) )
	{
		throw new JSError($strings['err_unknown']);
	}
	else
	{
		echo json_encode(array('message' => $strings['success']));
	}

} catch(JSError $e)
{
	echo "here";
}

// The err handler class
class JSError extends Exception {
	public function __construct($message) {
		echo json_encode(array('error' => $message));
		die();
	}
}