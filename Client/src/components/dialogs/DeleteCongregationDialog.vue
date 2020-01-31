<template>
  <v-dialog
    v-model="dialog"
    max-width="400px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        text
        color="error"
        v-t="'project.congregation.delete'"
      />
    </template>
    <v-card>
      <v-card-title>
        <span
          class="headline"
          v-t="'project.congregation.delete'"
        />
      </v-card-title>
      <v-card-text>
        <p
          class="subtitle-1"
          v-t="'project.congregation.deleteDescription'"
        />
      </v-card-text>
      <v-card-actions>
        <v-btn
          text
          v-t="'core.cancel'"
          @click.stop="dialog = false"
        />
        <v-spacer />
        <v-btn
          type="submit"
          text
          color="error"
          v-t="'core.delete'"
          @click.stop="deleteCongregation"
          :loading="loading"
        />
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { getModule } from 'vuex-module-decorators'
import { Component, Watch, Prop, Vue } from 'vue-property-decorator'
import i18n from '../../i18n'
import CongregationModule from '../../store/congregations'
import { Congregation } from '../../models'

@Component
export default class DeleteCongregationDialog extends Vue {
  private congregationModule = getModule(CongregationModule, this.$store)

  @Prop(Congregation)
  private readonly congregation?: Congregation

  private dialog: any = false
  private loading = false

  private async deleteCongregation() {
    this.loading = true

    if (this.congregation) {
      await this.congregationModule.deleteCongregation(this.congregation.id)
    }

    this.loading = false
    this.dialog = false
  }

}
</script>
