<template>
  <v-dialog
    v-model="dialog"
    max-width="600px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        text
        class="mt-2"
        v-t="'core.edit'"
      />
    </template>
    <v-card>
      <v-form
        v-model="valid"
        ref="form"
      >
        <v-card-title>
          <span
            class="headline"
            v-t="'project.invitation.invite'"
          />
        </v-card-title>
        <v-card-text>
          <p v-t="'project.participant.roleDescription'" />
          <v-select
            v-model="roleId"
            :items="roleValues"
            :rules="roleRules"
            item-text="name"
            item-value="value"
            :label="$tc('project.role.roles', 1)"
            :loading="loadingRoles"
            filled
            required
          />
        </v-card-text>
        <v-card-actions>
          <DeleteInvitationDialog :participationId="participation.id" />
          <v-spacer />
          <v-btn
            text
            @click.stop="dialog = false"
            v-t="'core.cancel'"
          />
          <v-btn
            :disabled="!valid || roleId === this.participation.roleId"
            type="submit"
            :loading="loading"
            text
            color="primary"
            @click.stop="save"
            v-t="'core.save'"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import { VForm } from 'vuetify/lib'
import i18n from '../../i18n'
import ProjectModule from '../../store/projects'
import RoleModule from '../../store/roles'
import InvitationModule from '../../store/invitations'
import DeleteInvitationDialog from './DeleteInvitationDialog.vue'
import { Participation, Role } from '../../models/definitions'

@Component({
  components: {
    DeleteInvitationDialog,
  }
})
export default class UpdateInvitationDialog extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)
  private invitationModule: InvitationModule = getModule(InvitationModule, this.$store)
  private roleModule: RoleModule = getModule(RoleModule, this.$store)

  @Prop(Participation)
  private readonly participation?: Participation

  private dialog: any = false
  private valid: any = null
  private loading: boolean = false
  private loadingRoles: boolean = true

  private roleId: string = ''
  private roleValues: any = []
  private roleRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
  ]

  private async created() {
    await this.roleModule.loadRoles();

    this.roleValues = this.projectModule.getActiveProject?.getRoles.map((role: Role) => {
      return {
        value: role.id,
        name: role.name,
      }
    })

    this.roleId = this.participation?.roleId || ''

    this.loadingRoles = false
  }

  private async save() {
    const projectId = this.$route.params.projectId

    if (this.valid && this.participation) {
      this.loading = true

      await this.invitationModule.updateInvitation({
        participationId: this.participation?.id,
        roleId: this.roleId
      })

      this.loading = false
      this.dialog = false
    }
  }

}
</script>
