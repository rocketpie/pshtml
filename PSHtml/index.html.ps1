Param( [string] $Name = "stranger" )

#set response content type 
"content-type: text/html"
$template = '
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en-us"> 
    <head>
        <title>Test</title>
    </head>
    <body>
       <h1>Hi there, $Name!</h1>
    </body>
</html>
'



$ExecutionContext.InvokeCommand.ExpandString($template) 
