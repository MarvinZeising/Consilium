<template>
  <v-container
    v-if="canView"
    fluid
    class="pa-0"
  >

    <!--//* Toolbar -->
    <!--//TODO: On mobile, replace this toolbar with a simpler one (only day view and date picker) -->
    <v-toolbar flat>
      <v-btn-toggle
        dense
        class="mr-4"
      >
        <v-btn
          v-t="'shift.today'"
          @click="setToday"
        />
      </v-btn-toggle>

      <v-btn
        fab
        text
        small
        @click="$refs.calendar.prev()"
      >
        <v-icon>keyboard_arrow_left</v-icon>
      </v-btn>

      <v-btn
        fab
        text
        small
        @click="$refs.calendar.next()"
      >
        <v-icon>keyboard_arrow_right</v-icon>
      </v-btn>

      <v-toolbar-title class="ml-4">{{ getTitle }}</v-toolbar-title>

      <v-spacer />


      <v-btn-toggle
        v-model="type"
        color="primary"
        mandatory
        dense
      >
        <v-btn
          value="month"
          v-t="'shift.month'"
        />
        <v-btn
          value="week"
          v-t="'shift.week'"
        />
        <v-btn
          value="day"
          v-t="'shift.day'"
        />
      </v-btn-toggle>

      <v-progress-linear
        :active="loading"
        :indeterminate="loading"
        absolute
        bottom
      />
    </v-toolbar>

    <!--//* Calendar -->
    <v-calendar
      id="calendar"
      ref="calendar"
      color="navbar"
      :style="`border-left:0; min-height:${getCalendarHeight}px;`"
      v-model="focus"
      :events="getEvents"
      :event-color="(event) => event.color"
      :type="type"
      :locale="userModule.getUser.language"
      :weekdays="weekdays"
      :event-more="10"
      @click:date="viewDay"
      @click:more="viewDay"
      @click:event="showEvent"
    />

    <CreateShiftDialog :date="focus" />

  </v-container>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import moment from 'moment'
import i18n from '../../i18n'
import UserModule from '../../store/users'
import PersonModule from '../../store/persons'
import ProjectModule from '../../store/projects'
import CategoryModule from '../../store/categories'
import ShiftModule from '../../store/shifts'
import CreateShiftDialog from '../../components/dialogs/CreateShiftDialog.vue'
import { Shift } from '../../models'

@Component({
  components: {
    CreateShiftDialog,
  }
})
export default class Calendar extends Vue {
  private userModule = getModule(UserModule, this.$store)
  private personModule = getModule(PersonModule, this.$store)
  private projectModule = getModule(ProjectModule, this.$store)
  private categoryModule = getModule(CategoryModule, this.$store)
  private shiftModule = getModule(ShiftModule, this.$store)

  private loading: boolean = true

  private type: string = 'month'
  private focus: string = moment().format('YYYY-MM-DD')
  private events: any = []
  private weekdays: number[] = [1, 2, 3, 4, 5, 6, 0]

  private start: any = null
  private end: any = null

  public get canView() {
    return this.personModule.getActiveRole?.calendarRead === true
  }

  private get canEdit() {
    return this.personModule.getActiveRole?.calendarWrite === true
  }

  private get getCalendarHeight() {
    return window.innerHeight - 128
  }

  private get getTitle() {
    switch (this.type) {
      case 'month':
        return moment(this.focus).format('MMMM YYYY')
      case 'week':
        const week = i18n.t('shift.week')
        return moment(this.focus).format(`[${week}] w, MMM gggg`)
      case 'day':
        return this.userModule.getUser?.formatDate(this.focus)
    }
    return ''
  }

  private get getEvents() {
    const project = this.projectModule.getActiveProject
    if (project) {
      let events: any[] = []

      project.getCategories
        .filter((x) => x.id === 'bd711f3f-f6f8-4e94-81ec-c724fa1c5d94')
        .forEach((x) => {
          events = events.concat(x.shifts)
        })

      events = events.map((x) => {
        return {
          name: '12 Participants',
          start: x.start,
          end: x.end,
          color: 'navbar',
        }
      })

      return events
    }
    return []
  }

  private async created() {
    this.loading = true

    await this.categoryModule.loadCategories()
    await this.shiftModule.loadShifts('bd711f3f-f6f8-4e94-81ec-c724fa1c5d94')

    this.loading = false
  }

  private viewDay({ date }: { date: string }) {
    this.focus = date
    this.type = 'day'
  }

  private setToday() {
    this.focus = moment().format('YYYY-MM-DD')
  }

}
</script>
