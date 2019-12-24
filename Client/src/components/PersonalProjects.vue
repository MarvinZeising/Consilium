<template>
  <v-flex xs12 sm10 md8 lg6 xl4>
    <h2
      class="headline mb-3"
      v-t="'project.projects'"
    />
    <v-card flat class="ma-2 mb-5">
      <v-card-text class="grey--text">
        {{ $t('project.projectsDescription') }}
      </v-card-text>
      <v-list>
        <v-list-item
          v-if="getProjects.length === 0"
          dark
          class="warning"
        >
          <span v-t="'project.noprojects'" />
        </v-list-item>
        <v-list-item
          three-line
          v-for="(project, index) in getProjects"
          :key="index"
          :class="isStatusInvited(project.id) ? 'warning' : isStatusRequested(project.id) ? 'info' : ''"
        >
          <v-list-item-content>
            <v-list-item-title v-text="project.name" />
            <v-list-item-subtitle v-t="'project.participationStatus.' + getParticipationStatus(project.id)" />
          </v-list-item-content>

          <v-list-item-action grow>
            <v-layout align-center justify-center>
              <v-btn
                v-if="isStatusInvited(project.id)"
                text
                class="mt-2"
                @click=""
              >
                Handle
              </v-btn>
              <CancelJoinRequestDialog
                v-else-if="isStatusRequested(project.id)"
                :projectParticipationId="getParticipationId(project.id)"
              />
              <v-btn
                v-else
                icon
                class="mt-2"
                @click=""
              >
                <v-icon>edit</v-icon>
              </v-btn>
            </v-layout>
          </v-list-item-action>
        </v-list-item>
      </v-list>
      <v-card-actions>
        <v-layout wrap>
          <JoinProjectDialog />
          <v-spacer />
          <v-btn
            text
            :to="{ name: 'createProject' }"
            v-t="'project.create'"
          />
        </v-layout>
      </v-card-actions>
    </v-card>
  </v-flex>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import ProjectModule from '../store/modules/projects'
import JoinProjectDialog from '../components/dialogs/JoinProjectDialog.vue'
import CancelJoinRequestDialog from '../components/dialogs/CancelJoinRequestDialog.vue'
import { Project, ProjectParticipationStatus } from '../models/definitions'

@Component({
  components: {
    JoinProjectDialog,
    CancelJoinRequestDialog,
  }
})
export default class ProjectalProjects extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)

  private get getProjects() {
    return this.projectModule.getProjects
  }

  private isStatusInvited(projectId: string) {
    return this.getParticipationStatus(projectId) === ProjectParticipationStatus.Invited
  }

  private isStatusRequested(projectId: string) {
    return this.getParticipationStatus(projectId) === ProjectParticipationStatus.Requested
  }

  private getParticipationId(projectId: string) {
    return this.getProjectParticipation(projectId)?.id
  }

  private getParticipationStatus(projectId: string) {
    const participation = this.getProjectParticipation(projectId)
    return participation?.status || 'none'
  }

  private getProjectParticipation(projectId: string) {
    return this.projectModule.getParticipations.find((p) => p.projectId === projectId)
  }

}
</script>
