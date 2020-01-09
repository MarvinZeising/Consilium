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
        <v-list-item v-if="personModule.getActivePerson.getParticipations.length === 0">
          <span v-t="'project.noProjects'" />
        </v-list-item>
        <v-list-item
          v-for="(participation, index) in personModule.getActivePerson.getParticipations"
          :key="index"
          :three-line="isStatusInvited(participation.project.id) || isStatusRequested(participation.project.id)"
          :class="isStatusInvited(participation.projectId) ? 'warning' : isStatusRequested(participation.projectId) ? 'info' : ''"
        >
          <v-list-item-content>
            <v-list-item-title v-text="participation.project.name" />
            <v-list-item-subtitle v-t="'project.participationStatus.' + getParticipationStatus(participation.projectId)" />
          </v-list-item-content>
          <v-list-item-action>
            <HandleProjectInvitationDialog
              v-if="isStatusInvited(participation.projectId)"
              :participation="getParticipation(participation.projectId)"
            />
            <DeleteJoinRequestDialog
              v-else-if="isStatusRequested(participation.projectId)"
              :participation="getParticipation(participation.projectId)"
            />
            <UpdateParticipationDialog
              v-else
              :participation="participation"
            />
          </v-list-item-action>
        </v-list-item>
      </v-list>
      <v-card-actions>
        <v-layout wrap>
          <CreateJoinRequestDialog />
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
import ProjectModule from '../store/projects'
import PersonModule from '../store/persons'
import CreateJoinRequestDialog from '../components/dialogs/CreateJoinRequestDialog.vue'
import DeleteJoinRequestDialog from '../components/dialogs/DeleteJoinRequestDialog.vue'
import HandleProjectInvitationDialog from '../components/dialogs/HandleProjectInvitationDialog.vue'
import UpdateParticipationDialog from '../components/dialogs/UpdateParticipationDialog.vue'
import { Project, ParticipationStatus } from '../models/definitions'

@Component({
  components: {
    CreateJoinRequestDialog,
    DeleteJoinRequestDialog,
    HandleProjectInvitationDialog,
    UpdateParticipationDialog,
  }
})
export default class PersonalProjects extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)
  private personModule: PersonModule = getModule(PersonModule, this.$store)

  private isStatusInvited(projectId: string) {
    return this.getParticipationStatus(projectId) === ParticipationStatus.Invited
  }

  private isStatusRequested(projectId: string) {
    return this.getParticipationStatus(projectId) === ParticipationStatus.Requested
  }

  private getParticipationStatus(projectId: string) {
    const participation = this.getParticipation(projectId)
    return participation?.status || 'none'
  }

  private getParticipation(projectId: string) {
    return this.personModule.getActivePerson?.participations.find((x) => x.projectId === projectId)
  }

}
</script>
