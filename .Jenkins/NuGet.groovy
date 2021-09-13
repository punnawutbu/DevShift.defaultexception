def folderName = 'NIO.DefaultException'
def projectName = 'NIO.DefaultException'
def gitUrl = 'http://linuxdev02.nio.ngg.local/dotnet/NIO.DefaultException.git'
def gitBranch = 'refs/heads/master'
def publishProject = 'NIO.DefaultException/NIO.DefaultException.csproj'
def testProject = 'NIO.DefaultException.Tests'
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