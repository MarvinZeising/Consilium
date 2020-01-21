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
        v-t="'project.request.cancel'"
      />
      <v-card-text v-t="'project.request.cancelDescription'" />
      <v-card-actions>
        <v-btn
          text
          @click.stop="dialog = false"
          v-t="'core.close'"
        />
        <v-spacer />
        <v-btn
          text
          color="primary"
          @click.stop="cancelJoinRequest"
          v-t="'project.request.cancel'"
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
import RequestModule from '../../store/requests'
import { Participation } from '../../models'

@Component
export default class DeleteJoinRequestDialog extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)
  private requestModule: RequestModule = getModule(RequestModule, this.$store)

  @Prop(Participation)
  private readonly participation?: Participation

  private form: any = null
  private dialog: any = null
  private loading: boolean = false

  private async cancelJoinRequest() {
    this.loading = true

    if (this.participation) {
      await this.requestModule.cancelRequest(this.participation)
    }

    this.loading = false
    this.dialog = false
  }
}
</script>
