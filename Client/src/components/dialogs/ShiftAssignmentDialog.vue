<template>

  <v-dialog
    v-model="dialog"
    :fullscreen="$vuetify.breakpoint.smAndDown"
    max-width="1000px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        icon
        @click="opened"
      >
        <v-icon>edit</v-icon>
      </v-btn>
    </template>
    <v-card>
      <v-toolbar
        flat
        :color="shift.isPlanned ? 'navbar' : shift.isScheduled ? 'green' : shift.isSuspended ? 'red' : ''"
      >
        <v-toolbar-title v-t="'shift.status.scheduling'" />
        <v-spacer />
        <v-btn icon>
          <v-icon>info</v-icon>
        </v-btn>
        <v-btn
          v-if="$vuetify.breakpoint.smAndDown"
          icon
          @click="dialog = false"
        >
          <v-icon>close</v-icon>
        </v-btn>
      </v-toolbar>

      <ShiftAssignmentDialogPlanning
        :shift="shift"
        :assignments="assignments"
      />

      <v-divider />

      <v-card-actions v-if="shift.isPlanned">
        <v-btn
          text
          color="error"
          v-t="'shift.status.unplan'"
          :loading="unplanning"
          @click.stop="unplan"
        />
      </v-card-actions>

      <v-card-actions v-if="shift.isScheduled">
        <v-btn
          text
          color="error"
          v-t="'shift.status.unschedule'"
          :loading="unscheduling"
          :disabled="saving || suspending || callingOff"
          @click.stop="unschedule"
        />
        <v-spacer />
        <v-btn
          text
          color="accent"
          v-t="'shift.status.suspend'"
          :loading="suspending"
          :disabled="saving || unscheduling || callingOff"
          @click.stop="suspend"
        />
        <v-btn
          text
          color="error"
          v-t="'shift.status.callOff'"
          :loading="callingOff"
          :disabled="saving || unscheduling || suspending"
          @click.stop="callOff"
        />
      </v-card-actions>

      <v-card-actions>
        <v-btn
          text
          v-t="'core.cancel'"
          @click.stop="dialog = false"
        />
        <v-spacer />
        <v-btn
          text
          v-t="'core.reset'"
          :disabled="saving || releasing || isSaved"
          @click.stop="reset"
        />
        <v-btn
          text
          v-t="'core.save'"
          :loading="saving"
          :disabled="releasing || isSaved"
          @click.stop="save"
        />
        <v-btn
          v-if="!shift.isScheduled"
          text
          color="primary"
          v-t="'shift.status.schedule'"
          :loading="scheduling"
          :disabled="saving"
          @click.stop="schedule"
        />
      </v-card-actions>

    </v-card>
  </v-dialog>

</template>

<script lang="ts">
import { Vue, Component, Prop, Emit } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import PersonModule from '../../store/persons'
import ShiftModule from '../../store/shifts'
import ShiftAssignmentDialogPlanning from './ShiftAssignmentDialogPlanning.vue'
import { Shift, Person, ShiftStatus } from '../../models'

@Component({
  components: {
    ShiftAssignmentDialogPlanning,
  }
})
export default class ShiftAssignmentDialog extends Vue {
  private personModule = getModule(PersonModule, this.$store)
  private shiftModule = getModule(ShiftModule, this.$store)

  @Prop(Shift)
  private readonly shift?: Shift

  private dialog = false
  private saving = false
  private scheduling = false
  private unscheduling = false
  private unplanning = false
  private suspending = false
  private callingOff = false

  private savedAssignments: {
    [personId: string]: {
      teamId: string,
      isCaptain: boolean
    }
  } = {}
  private assignments: {
    [personId: string]: {
      teamId: string,
      isCaptain: boolean
    }
  } = {}

  private get canEdit() {
    const role = this.personModule.getActiveRole
    if (role && role.calendarWrite) {
      const eligibility = role.eligibilities.find((x) => x.categoryId === this.shift?.categoryId)
      if (eligibility) {
        return eligibility.shiftsWrite
      }
    }
    return false
  }

  private get isSaved() {
    if (this.assignments.length !== this.savedAssignments.length) {
      return false
    }

    for (const personId of Object.keys(this.assignments)) {
      if (this.assignments[personId]?.teamId !== this.savedAssignments[personId]?.teamId
       || this.assignments[personId]?.isCaptain !== this.savedAssignments[personId]?.isCaptain) {
        return false
      }
    }

    return true
  }

  private opened() {
    Vue.set(this, 'assignments', {})
    Vue.set(this, 'savedAssignments', {})

    if (this.shift?.attendees) {
      for (const attendee of this.shift.attendees) {
        Vue.set(this.assignments, attendee.personId, {
          teamId: attendee.teamId,
          isCaptain: attendee.isCaptain,
        })
        Vue.set(this.savedAssignments, attendee.personId, {
          teamId: attendee.teamId,
          isCaptain: attendee.isCaptain,
        })
      }
    }
  }

  private async reset() {
    const copy = {}
    for (const personId of Object.keys(this.savedAssignments)) {
      if (this.savedAssignments[personId]) {
        Vue.set(copy, personId, {
          teamId: this.savedAssignments[personId]?.teamId,
          isCaptain: this.savedAssignments[personId].isCaptain,
        })
      }
    }
    Vue.set(this, 'assignments', copy)
  }

  private async save() {
    if (this.shift) {
      this.saving = true

      const assignments = Object.keys(this.assignments)
        .filter((personId) => this.assignments[personId] !== undefined)
        .map((personId) => {
          return {
            personId,
            ...this.assignments[personId],
          }
        })

      await this.shiftModule.makeAssignment({
        shiftId: this.shift?.id,
        assignments,
      })

      const copy = {}
      for (const personId of Object.keys(this.assignments)) {
        if (this.assignments[personId]) {
          Vue.set(copy, personId, {
            teamId: this.assignments[personId]?.teamId,
            isCaptain: this.assignments[personId].isCaptain,
          })
        }
      }
      Vue.set(this, 'savedAssignments', copy)

      this.saving = false
      this.emitSaved()
      this.dialog = true
    }
  }

  private async unplan() {
    if (this.shift) {
      this.unplanning = true

      this.shiftModule.updateShiftStatus({
        shiftId: this.shift.id,
        status: ShiftStatus.draft
      })

      this.unplanning = false
    }
  }

  private async schedule() {
    if (this.shift) {
      this.scheduling = true

      if (!this.isSaved) {
        await this.save()
      }

      this.shiftModule.updateShiftStatus({
        shiftId: this.shift.id,
        status: ShiftStatus.scheduled
      })

      this.scheduling = false
    }
  }

  private async unschedule() {
    if (this.shift) {
      this.unscheduling = true

      this.shiftModule.updateShiftStatus({
        shiftId: this.shift.id,
        status: ShiftStatus.planned
      })

      this.unscheduling = false
    }
  }

  private async suspend() {
    if (this.shift) {
      this.suspending = true

      this.shiftModule.updateShiftStatus({
        shiftId: this.shift.id,
        status: ShiftStatus.suspended
      })

      this.suspending = false
    }
  }

  private async callOff() {
    if (this.shift) {
      this.callingOff = true

      this.shiftModule.updateShiftStatus({
        shiftId: this.shift.id,
        status: ShiftStatus.calledOff
      })

      this.callingOff = false
    }
  }

  @Emit('saved') private emitSaved() { /* nothing to do */ }

}
</script>
