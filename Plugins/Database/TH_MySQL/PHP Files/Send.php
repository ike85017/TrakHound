<?php
$servername = $_POST['server'];
$username = $_POST['user'];
$password = $_POST['password'];
$database = $_POST['db'];

// Create connection
$conn = new mysqli($servername, $username, $password, $database);

// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

$sql = $_POST["query"];

if ($conn->query($sql) === TRUE) {
    echo "true";
} else {
    echo "false";
}

$conn->close();
?> 