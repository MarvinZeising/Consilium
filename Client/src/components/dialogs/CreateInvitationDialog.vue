<template>
  <v-dialog
    v-model="dialog"
    max-width="600px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        text
        v-t="'project.invitation.invite'"
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
          <p
            class="subtitle-1"
            v-t="'project.invitation.inviteDescription'"
          />
          <p
            class="subtitle-1"
            v-t="'project.invitation.idDescription'"
          />
          <v-text-field
            v-model="personId"
            :rules="personIdRules"
            :label="$t('core.id')"
            filled
            required
          />
          <p v-t="'project.invitation.roleDescription'" />
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
          <v-spacer />
          <v-btn
            text
            @click.stop="dialog = false"
            v-t="'core.cancel'"
          />
          <v-btn
            :disabled="!valid"
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
import { Vue, Component } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import { VForm } from 'vuetify/lib'
import i18n from '../../i18n'
import ProjectModule from '../../store/projects'
import InvitationModule from '../../store/invitations'
import RequestModule from '../../store/requests'
import RoleModule from '../../store/roles'
import { Role, Exceptions } from '@/models'

@Component
export default class CreateInvitationDialog extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)
  private invitationModule: InvitationModule = getModule(InvitationModule, this.$store)
  private requestModule: RequestModule = getModule(RequestModule, this.$store)
  private roleModule: RoleModule = getModule(RoleModule, this.$store)

  private dialog: any = false
  private valid: any = null
  private loading: boolean = false
  private loadingRoles: boolean = true

  private personId: string = ''
  private personIdRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => /^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i.test(v) || i18n.t('core.fieldInvalidGuid'),
  ]
  private roleId: string = ''
  private roleValues: any = []
  private roleRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
  ]

  private async created() {
    await this.roleModule.loadRoles();

    const project = this.projectModule.getActiveProject
    if (project) {
      this.roleValues = project.getRoles.map((role: Role) => {
        return {
          value: role.id,
          name: role.name,
        }
      })

      this.personIdRules.push((v: string) => {
        const invitations = project.getInvitations
        const requests = project.getRequests
        const permanents = project.getParticipations
        const allParticipantIds = permanents.concat(invitations).concat(requests).map((x) => x.id)
        return !allParticipantIds?.includes(v) || this.$t('project.invitation.duplicate')
      })
    }

    this.loadingRoles = false
  }

  private throwValidationError(message: string) {
      const form: any = this.$refs.form
      const thisPersonId = this.personId
      this.personIdRules.push((v: string) => v !== thisPersonId || i18n.t(message))
      form.validate()
  }

  private async save() {
    this.loading = true
    const projectId = this.$route.params.projectId

    const response = await this.invitationModule.createInvitation({
      personId: this.personId,
      roleId: this.roleId
    })
    if (response === Exceptions.PersonNotFound) {
      this.throwValidationError('person.notFound')
    } else if (response) {
      this.throwValidationError('core.generalError')
    } else {
      this.dialog = false
    }

    this.loading = false
  }

}
</script>
