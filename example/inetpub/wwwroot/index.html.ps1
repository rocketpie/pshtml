$template = '''
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en-us"> 
    <head>
        <title>Test</title>
    </head>
    <body>
        $name
    </body>
</html>
'''


$name = "peter"


$ExecutionContext.InvokeCommand.ExpandString($template) 
