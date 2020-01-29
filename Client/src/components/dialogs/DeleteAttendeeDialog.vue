<template>
  <v-dialog
    v-model="dialog"
    max-width="400px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        text
        v-t="'shift.attendee.cancel'"
        :loading="loading"
      />
    </template>
    <v-card>
      <v-form v-model="valid">
        <v-card-title>
          <span
            class="headline"
            v-t="'shift.attendee.delete'"
          />
        </v-card-title>
        <v-card-text>
          <p
            class="subtitle-1"
            v-t="'shift.attendee.deleteDescription'"
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
            text
            type="submit"
            color="error"
            v-t="'shift.attendee.delete'"
            :loading="loading"
            :disabled="!valid"
            @click.stop="deleteAttendee"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Vue, Component, Watch, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import ApplicationModule from '../../store/applications'
import { Attendee } from '../../models'

@Component
export default class DeleteAttendeeDialog extends Vue {
  private applicationModule = getModule(ApplicationModule, this.$store)

  @Prop(Attendee)
  private readonly attendee?: Attendee

  private valid: any = false
  private dialog: boolean = false
  private loading: boolean = false

  private async deleteAttendee() {
    if (this.attendee) {
      this.loading = true

      await this.applicationModule.deleteAttendee(this.attendee)

      this.loading = false
    }

    this.dialog = false
  }

}
</script>
