<template>
  <v-dialog
    v-model="dialog"
    max-width="400"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        text
        class="mt-2"
        @click.stop="dialog = true"
        v-t="'core.handle'"
      />
    </template>
    <v-card>
      <v-card-title
        class="headline"
        v-t="'project.handleInvitation'"
      />
      <v-card-text v-t="'project.handleInvitationDescription'" />
      <v-card-actions>
        <v-btn
          text
          @click="dialog = false"
          v-t="'core.cancel'"
        />
        <v-spacer></v-spacer>
        <v-btn
          text
          color="error"
          @click="declineInvitation"
          v-t="'core.decline'"
          :loading="loading"
        />
        <v-btn
          text
          color="primary"
          @click="acceptInvitation"
          v-t="'core.accept'"
          :loading="loading"
        />
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { getModule } from 'vuex-module-decorators'
import { Vue, Component, Prop } from 'vue-property-decorator'
import i18n from '../../i18n'
import ProjectModule from '../../store/projects'

@Component
export default class HandleProjectInvitationDialog extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)

  @Prop(String)
  private readonly participationId?: string

  private form: any = null
  private dialog: any = null
  private loading: boolean = false

  private async acceptInvitation() {
    this.loading = true

    if (this.participationId) {
      await this.projectModule.acceptInvitation(this.participationId)
    }

    this.dialog = false
  }

  private async declineInvitation() {
    this.loading = true

    if (this.participationId) {
      await this.projectModule.declineInvitation(this.participationId)
    }

    this.dialog = false
  }

}
</script>
