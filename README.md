# dotNet-project-templates
Since the release of .Net 6, Renaming solution has become very easy. For this, I created a WebApi project template in .Net 7 with complete unit test and xml doc that has all the basic non-requirement functions in building a modular low coupling and high cohension application. 
This can save time and help to focus on the functional requirement while starting a new project.  JWT auth Module is included.
To use this template, all you need to do is 
1) Right click on the solunction name (KOProject.sln) and rename it to your project.
2) set the properties of the option for configuring your auth module (sql server ConnectionString, jwt secrete key, refresh and access token life span).
3) Run efcore command to update a auth database.
