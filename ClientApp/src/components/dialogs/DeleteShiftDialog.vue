<template>
  <v-dialog v-model="dialog" max-width="400px">
    <template v-slot:activator="{ on }">
      <v-btn v-on="on" text color="error" :loading="loading" v-t="'shift.delete'" />
    </template>
    <v-card>
      <v-form v-model="valid">
        <v-card-title>
          <span class="headline" v-t="'shift.delete'" />
        </v-card-title>
        <v-card-text>
          <p class="subtitle-1" v-t="'shift.deleteDescription'" />
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn text v-t="'core.cancel'" @click.stop="dialog = false" />
          <v-btn
            text
            type="submit"
            color="error"
            v-t="'core.delete'"
            :loading="loading"
            :disabled="!valid"
            @click.prevent="deleteShift"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Vue, Component, Watch, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import ShiftModule from '../../store/shifts'
import { Shift } from '../../models'

@Component
export default class DeleteShiftDialog extends Vue {
  private shiftModule = getModule(ShiftModule, this.$store)

  @Prop(Shift)
  private readonly shift?: Shift

  private valid = false
  private dialog = false
  private loading = false

  private async deleteShift() {
    if (this.shift) {
      this.loading = true

      await this.shiftModule.deleteShift(this.shift)

      this.loading = false
    }

    this.dialog = false
  }
}
</script>
