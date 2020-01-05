<template>
  <v-flex xs12 sm10 md8 lg6 xl4>
    <h2
      class="headline mb-3"
      v-t="'project.invitation.invitations'"
    />
    <v-card
      flat
      class="ma-2 mb-5"
      :loading="loading"
    >
      <v-card-text
        class="grey--text"
        v-t="'project.invitation.description'"
      />
      <v-list v-if="projectModule.getInvitations">
        <v-list-item v-if="projectModule.getInvitations.length === 0">
          <span v-t="'project.invitation.noInvitations'" />
        </v-list-item>
        <v-list-item
          v-for="(participation, index) in projectModule.getInvitations"
          :key="index"
          three-line
        >
          <v-list-item-content>
            <v-list-item-title v-text="participation.person.fullName()" />
            <v-list-item-subtitle v-text="getRoleText(participation.role.name)" />
            <v-list-item-subtitle v-text="getCreationText(participation.createdTime)" />
          </v-list-item-content>
          <v-list-item-action>
            <v-btn
              text
              class="mt-2"
              v-t="'core.edit'"
            />
          </v-list-item-action>
        </v-list-item>
      </v-list>
      <v-card-actions>
        <v-spacer />
        <CreateInvitationDialog />
      </v-card-actions>
    </v-card>
  </v-flex>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import moment from 'moment'
import ProjectModule from '../store/projects'
import { Person, ParticipationStatus, Gender } from '../models/definitions'
import CreateInvitationDialog from './dialogs/CreateInvitationDialog.vue'

@Component({
  components: {
    CreateInvitationDialog,
  }
})
export default class ParticipantsInvitations extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)

  private loading: boolean = true

  @Watch('$route')
  private async onRouteChanged(val: string, oldVal: string) {
    await this.init()
  }

  private async created() {
    this.init()
  }

  private async init() {
    await this.projectModule.loadInvitations();

    this.loading = false
  }

  private getRoleText(roleName: string) {
    return this.$t('project.invitation.roleName', { name: roleName })
  }

  private getCreationText(createdTime: string) {
    return this.$t('project.invitation.invitedOn', {
      date: moment(createdTime).format('ddd, MMM Do YYYY, h:mm a')
    })
  }

  private  getPerson(personId: string) {
    return 'Stinkemarv'
    // return this.projectModule.getParticipations.find((x) => x.id === personId)
  }

}
</script>
