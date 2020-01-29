<template>

  <v-menu
    v-if="getShift"
    v-model="model.model"
    :close-on-content-click="false"
    :activator="model.element"
    offset-x
    offset-overflow
    :content-class="$vuetify.breakpoint.xsOnly ? 'menu-fullscreen' : ''"

  >
    <ShiftOverviewMenuDraft
      v-if="getShift.isDraft"
      :shift="getShift"
    />
    <v-card
      v-else
      min-width="350px"
      max-width="500px"
      :elevation="$vuetify.breakpoint.xsOnly ? 8 : undefined"
    >
      <v-toolbar
        :color="getEventColor"
        dark
        flat
        short
      >
        <v-spacer></v-spacer>
        <ShiftAssignmentDialog
          v-if="canEdit && !getShift.isDraft"
          :shift="getShift"
          @saved="model.model = true"
        />
        <UpdateShiftDialog
          v-if="canEdit && getShift.isDraft"
          :shift="getShift"
        />
        <v-btn
          icon
          :title="$t('core.close')"
          @click="model.model = false"
        >
          <v-icon>close</v-icon>
        </v-btn>
      </v-toolbar>

      <v-toolbar
        :color="getEventColor"
        dark
        flat
        short
      >
        <v-toolbar-title>{{ getShift.getTimespan(userModule.getUser) }}</v-toolbar-title>
        <v-spacer />
        <v-toolbar-title>{{ userModule.getUser.formatDate(getShift.date) }}</v-toolbar-title>
      </v-toolbar>

      <ShiftOverviewMenuActions
        :shift="getShift"
      />

      <v-list-group
        v-if="getShift.attendees.length > 0"
        :value="showAttendees"
        color="/*none*/"
      >
        <template v-slot:activator>
          <v-list-item-title>
            {{ $tc('shift.attendee.attendees', 2) }}
          </v-list-item-title>
        </template>
        <v-list-item
          v-for="(attendee, index) in getShift.getAttendees"
          :key="index"
          @click="openPerson()"
        >
          <v-list-item-content>
            <v-list-item-title>
              {{ attendee.person.getFullName }}
              <v-icon small v-if="attendee.isCaptain">flag</v-icon>
            </v-list-item-title>
            <v-list-item-subtitle>{{ attendee.person.congregation.name }}</v-list-item-subtitle>
          </v-list-item-content>
          <v-list-item-action>
            <v-icon>info</v-icon>
          </v-list-item-action>
        </v-list-item>
      </v-list-group>

      <v-list-group
        v-if="getShift.applications.length > 0"
        :value="showApplicants"
        color="/*none*/"
      >
        <template v-slot:activator>
          <v-list-item-title>
            {{ $tc('shift.application.applicants', 2) }}
          </v-list-item-title>
        </template>
        <v-list-item
          v-for="(application, index) in getShift.getApplications"
          :key="index"
          @click="openPerson()"
        >
          <v-list-item-avatar>
            <v-icon color="accent">emoji_people</v-icon>
          </v-list-item-avatar>
          <v-list-item-content>
            <v-list-item-title>{{ application.person.getFullName }}</v-list-item-title>
            <v-list-item-subtitle>6 applications, 3 approved</v-list-item-subtitle>
          </v-list-item-content>
        </v-list-item>
      </v-list-group>

    </v-card>
  </v-menu>

</template>

<style lang="scss" scoped>
  .menu-fullscreen {
    top: 0px !important;
    left: 0px !important;
    min-width: 100% !important;
    padding: 12px !important;
    box-shadow: none;
  }
</style>

<script lang="ts">
import { Vue, Component, Prop, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import UserModule from '../../store/users'
import PersonModule from '../../store/persons'
import UpdateShiftDialog from './UpdateShiftDialog.vue'
import HandlePlanShiftDialog from './HandlePlanShiftDialog.vue'
import ShiftAssignmentDialog from './ShiftAssignmentDialog.vue'
import ShiftOverviewMenuDraft from './ShiftOverviewMenuDraft.vue'
import ShiftOverviewMenuActions from './ShiftOverviewMenuActions.vue'
import { Shift } from '../../models'

@Component({
  components: {
    UpdateShiftDialog,
    HandlePlanShiftDialog,
    ShiftAssignmentDialog,
    ShiftOverviewMenuDraft,
    ShiftOverviewMenuActions,
  },
})
export default class CreateTeamDialog extends Vue {
  private userModule = getModule(UserModule, this.$store)
  private personModule = getModule(PersonModule, this.$store)

  @Prop(Object)
  private readonly model?: {
    model: any,
    element: any,
    event: any,
  }

  private showApplicants = true
  private showAttendees = true

  private get canEdit() {
    const role = this.personModule.getActiveRole
    if (role && role.calendarWrite) {
      const eligibility = role.eligibilities.find((x) => x.categoryId === this.getShift?.categoryId)
      if (eligibility) {
        return eligibility.shiftsWrite
      }
    }
    return false
  }

  private get getShift(): Shift | undefined {
    return this.model?.event.shift
  }

  private get getEventColor() {
     if (this.getShift?.isScheduled) {
       return 'green'
     }
     return 'navbar'
  }

  private openPerson() {}

}
</script>
