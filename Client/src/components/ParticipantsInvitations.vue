<template>
  <v-flex
    v-if="canView"
    xs12 sm10 md8 lg6 xl4
  >
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
      <v-list v-if="invitationModule.getInvitations">
        <v-list-item v-if="invitationModule.getInvitations.length === 0">
          <span v-t="'project.invitation.noInvitations'" />
        </v-list-item>
        <v-list-item
          v-for="(participation, index) in invitationModule.getInvitations"
          :key="index"
          three-line
        >
          <v-list-item-content>
            <v-list-item-title v-text="participation.person.fullName()" />
            <v-list-item-subtitle v-text="getRoleText(participation.role.name)" />
            <v-list-item-subtitle v-text="getCreationText(participation.createdTime)" />
          </v-list-item-content>
          <v-list-item-action>
            <UpdateInvitationDialog :participation="participation" />
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
import UserModule from '../store/users'
import PersonModule from '../store/persons'
import InvitationModule from '../store/invitations'
import { Person, ParticipationStatus, Gender } from '../models/definitions'
import CreateInvitationDialog from './dialogs/CreateInvitationDialog.vue'
import UpdateInvitationDialog from './dialogs/UpdateInvitationDialog.vue'

@Component({
  components: {
    CreateInvitationDialog,
    UpdateInvitationDialog,
  }
})
export default class ParticipantsInvitations extends Vue {
  private userModule: UserModule = getModule(UserModule, this.$store)
  private personModule: PersonModule = getModule(PersonModule, this.$store)
  private invitationModule: InvitationModule = getModule(InvitationModule, this.$store)

  private loading: boolean = true

  private get canView() {
    return this.personModule.getActiveRole?.participantsWrite === true
  }

  @Watch('personModule.getActivePerson')
  private async onPersonChanged(val: string, oldVal: string) {
    if (this.canView) {
      await this.init()
    }
  }

  @Watch('$route')
  private async onRouteChanged(val: string, oldVal: string) {
    if (this.canView) {
      await this.init()
    }
  }

  private async created() {
    if (this.canView) {
      await this.init()
    }
  }

  private async init() {
    this.loading = true

    await this.invitationModule.loadInvitations();

    this.loading = false
  }

  private getRoleText(roleName: string) {
    return this.$t('project.invitation.roleName', { name: roleName })
  }

  private getCreationText(createdTime: string) {
    const user = this.userModule.getUser
    if (user) {
      return this.$t('project.invitation.invitedOn', {
        date: moment(createdTime).format(`${user?.dateFormat}, ${user?.timeFormat}`)
      })
    }
    return ''
  }

}
</script>
