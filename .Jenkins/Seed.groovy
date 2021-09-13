def folderName = 'NIO.DefaultException'
def gitUrl = 'http://linuxdev02.nio.ngg.local/dotnet/NIO.DefaultException.git'
 
folder(folderName)

job("$folderName/Seed") {
  logRotator(-1, 10)
  wrappers {
    timestamps()
  }
 
  scm {
    git {
      branch('refs/heads/master')
      remote {
        url(gitUrl)
        credentials('gitlab')
      }
    }
  }
 
  steps {
    triggers {
      gitlab {
        triggerOnPush(true)
        triggerOnMergeRequest(false)
        triggerOnAcceptedMergeRequest(false)
        triggerOnClosedMergeRequest(false)
        triggerOpenMergeRequestOnPush('never')
        triggerOnNoteRequest(false)
        noteRegex('')
        skipWorkInProgressMergeRequest(false)
        ciSkip(false)
        setBuildDescription(false)
        addNoteOnMergeRequest(false)
        addCiMessage(false)
        addVoteOnMergeRequest(false)
        acceptMergeRequestOnSuccess(false)
        branchFilterType('RegexBasedFilter')
        includeBranchesSpec('')
        excludeBranchesSpec('')
        targetBranchRegex('master')
        mergeRequestLabelFilterConfig {
          include('')
          exclude('')
        }
        secretToken('daa5523f369e1f2abc6b20fae9350c2d')
        triggerOnPipelineEvent(false)
      }
    }
 
    dsl {
      external "**/*.groovy"
    }
 
    publishers {
        wsCleanup()
    }
  }
}