<template>
  <v-dialog
    v-model="dialog"
    max-width="500"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        text
        class="mt-2"
        @click.stop="dialog = true"
        v-t="'core.cancel'"
      />
    </template>
    <v-card>
      <v-card-title
        class="headline"
        v-t="'project.cancelRequest'"
      />
      <v-card-text v-t="'project.cancelRequestDescription'" />
      <v-card-actions>
        <v-btn
          text
          @click="dialog = false"
          v-t="'project.cancelRequestOtherButton'"
        />
        <v-spacer></v-spacer>
        <v-btn
          text
          color="primary"
          @click="cancelJoinRequest"
          v-t="'project.cancelRequestSubmit'"
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
export default class CancelJoinRequestDialog extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)

  @Prop(String)
  private readonly participationId?: string

  private form: any = null
  private dialog: any = null
  private loading: boolean = false

  private async cancelJoinRequest() {
    this.loading = true

    if (this.participationId) {
      await this.projectModule.cancelJoinRequest(this.participationId)
    }

    this.dialog = false
  }
}
</script>
