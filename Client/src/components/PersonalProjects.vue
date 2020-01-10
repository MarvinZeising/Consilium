<template>
  <v-flex xs12 sm10 md8 lg6 xl4>
    <h2 class="headline mb-3">
      {{ $tc('project.projects', 2) }}
    </h2>
    <v-card flat class="ma-2 mb-5">
      <v-card-text class="grey--text">
        {{ $t('project.projectsDescription') }}
      </v-card-text>
      <v-list>
        <v-list-item v-if="personModule.getActivePerson.getParticipations.length === 0">
          {{ $tc('project.projects', 0) }}
        </v-list-item>
        <v-list-item
          v-for="(participation, index) in personModule.getActivePerson.getParticipations"
          :key="index"
          :three-line="isStatusInvited(participation) || isStatusRequested(participation)"
          :class="isStatusInvited(participation) ? 'warning' : isStatusRequested(participation) ? 'info' : ''"
        >
          <v-list-item-content>
            <v-list-item-title v-text="participation.project.name" />
            <v-list-item-subtitle
              v-if="isStatusInvited(participation) || isStatusRequested(participation) || isStatusInactive(participation)"
              v-t="'project.participationStatus.' + participation.status"
            />
          </v-list-item-content>
          <v-list-item-action>
            <HandleInvitationDialog
              v-if="isStatusInvited(participation)"
              :participation="participation"
            />
            <DeleteJoinRequestDialog
              v-else-if="isStatusRequested(participation)"
              :participation="participation"
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
import HandleInvitationDialog from '../components/dialogs/HandleInvitationDialog.vue'
import UpdateParticipationDialog from '../components/dialogs/UpdateParticipationDialog.vue'
import { Project, ParticipationStatus, Participation } from '../models/definitions'

@Component({
  components: {
    CreateJoinRequestDialog,
    DeleteJoinRequestDialog,
    HandleInvitationDialog,
    UpdateParticipationDialog,
  }
})
export default class PersonalProjects extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)
  private personModule: PersonModule = getModule(PersonModule, this.$store)

  private isStatusInvited(participation: Participation) {
    return participation.status === ParticipationStatus.Invited
  }

  private isStatusRequested(participation: Participation) {
    return participation.status === ParticipationStatus.Requested
  }

  private isStatusInactive(participation: Participation) {
    return participation.status === ParticipationStatus.Inactive
  }

}
</script>
