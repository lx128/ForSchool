<?PHP
$serveur = "localhost";
$bdd = "???????";
$bdduser = "root";
$bddpass = "";

	//Connection à la base
	$connexion = mysql_connect($serveur,$bdduser,$bddpass);
	if ($connexion==0)
	{
		printf ("Erreur BDD. Echec de connection.");
		exit;
	}
	/* selection de la base de donnée mysql */
	mysql_select_db($bdd, $connexion);

	$query = sprintf("SELECT room_name,start_time,end_time FROM `grr_entry`, `grr_room` WHERE grr_room.id=room_id AND start_time<(unix_timestamp()+1200) AND end_time>unix_timestamp() ORDER by room_name");
//echo $query;
	$ress=mysql_query($query);
	$row = mysql_fetch_assoc($ress);
	$length = mysql_fetch_lengths($ress);

	while ($length!=0)
	{
		echo $row['room_name'];
		echo ";";
		echo $row['start_time'];
		echo ";";
		echo $row['end_time'];
		echo ";\n";
		$row = mysql_fetch_assoc($ress);
		$length = mysql_fetch_lengths($ress);
	}
?>