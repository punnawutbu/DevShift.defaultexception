def folderName = 'DevShift.DefaultException'
def projectName = 'DevShift.DefaultException'
def gitUrl = 'https://github.com/punnawutbu/DevShift.defaultexception.git'
def gitBranch = 'refs/heads/master'
def publishProject = 'DevShift.DefaultException/DevShift.DefaultException.csproj'
def testProject = 'DevShift.DefaultException.Tests'
def versionPrefix = "1.0"

folder(projectName)
pipelineJob("$projectName/Release") {
 logRotator(-1, 10)
 triggers {
   upstream("$projectName/Seed", 'SUCCESS')
 }
 definition {
   parameters {
     choiceParam('Release', ['Beta', 'General Availability (GA)'], '')
   }
   cps {
     sandbox()
     script("""
       @Library('jenkins-shared-libraries')_

       def _versionSuffix = Release == 'Beta' ? 'beta' : ''

       nuGet {
         projectName = '$projectName'
         gitUri = '$gitUrl'
         gitBranch = '$gitBranch'
         publishProject = '$publishProject'
         testProject = '$testProject'
         versionPrefix = '$versionPrefix'
         versionSuffix = _versionSuffix
       }
    """)
   }
 }
}
