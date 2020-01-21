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
        v-t="'core.handle'"
        @click="opened"
      />
    </template>
    <v-card>
      <v-form
        ref="form"
        v-model="valid"
      >
        <v-card-title
          class="headline"
          v-t="'project.request.handle'"
        />
        <v-card-text>
          <p v-t="'project.request.handleDescription'" />
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
          <v-btn
            text
            @click.stop="dialog = false"
            v-t="'core.cancel'"
          />
          <v-spacer />
          <v-btn
            text
            color="error"
            @click.stop="declineRequest"
            v-t="'core.decline'"
            :loading="declining"
            :disabled="accepting"
          />
          <v-btn
            text
            color="primary"
            v-t="'core.accept'"
            @click.stop="acceptRequest"
            :loading="accepting"
            :disabled="declining || !valid"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { getModule } from 'vuex-module-decorators'
import { Vue, Component, Prop } from 'vue-property-decorator'
import i18n from '../../i18n'
import InvitationModule from '../../store/invitations'
import ProjectModule from '../../store/projects'
import RoleModule from '../../store/roles'
import RequestModule from '../../store/requests'
import { Participation, Role } from '../../models'

@Component
export default class HandleJoinRequestDialog extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)
  private roleModule: RoleModule = getModule(RoleModule, this.$store)
  private requestModule: RequestModule = getModule(RequestModule, this.$store)

  @Prop(Participation)
  private readonly participation?: Participation

  private valid: any = null
  private dialog: any = null
  private loadingRoles: boolean = false
  private accepting: boolean = false
  private declining: boolean = false

  private roleId: string = ''
  private roleValues: any = []
  private roleRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
  ]

  private async opened() {
    this.loadingRoles = true

    await this.roleModule.loadRoles()

    const project = this.projectModule.getActiveProject
    if (project) {
      this.roleValues = project.getRoles.map((role: Role) => {
        return {
          value: role.id,
          name: role.name,
        }
      })
    }

    this.loadingRoles = false
  }

  private async acceptRequest() {
    this.accepting = true

    if (this.participation) {
      this.participation.roleId = this.roleId
      await this.requestModule.acceptRequest(this.participation)
    }

    this.accepting = false
    this.dialog = false
  }

  private async declineRequest() {
    this.declining = true

    if (this.participation) {
      await this.requestModule.declineRequest(this.participation)
    }

    this.declining = false
    this.dialog = false
  }

}
</script>
