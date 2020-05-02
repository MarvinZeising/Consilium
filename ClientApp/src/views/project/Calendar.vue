<template>
  <v-container v-if="canView" fluid class="pa-0" v-resize="setCalendarHeight">
    <!-- // * big toolbar -->
    <v-toolbar class="d-none d-sm-block" flat>
      <v-btn v-t="'shift.today'" class="mr-4 d-none d-md-flex" outlined @click="setToday" />

      <v-btn fab text small @click="$refs.calendar.prev()">
        <v-icon>keyboard_arrow_left</v-icon>
      </v-btn>

      <v-btn fab text small @click="$refs.calendar.next()">
        <v-icon>keyboard_arrow_right</v-icon>
      </v-btn>

      <v-toolbar-title class="ml-4 d-none d-md-flex">{{ getTitle }}</v-toolbar-title>

      <v-spacer />

      <CategoriesControl
        :model="categoriesModel"
        @change="(selected) => categoryModel.value = selected[0]"
      />

      <CalendarTypeControl :model="calendarTypeModel" />

      <v-progress-linear :active="loading" :indeterminate="loading" absolute bottom />
    </v-toolbar>

    <!-- // * small toolbar -->
    <v-toolbar class="d-xs-block d-sm-none" flat>
      <v-btn fab text small @click="$refs.calendar.prev()">
        <v-icon>keyboard_arrow_left</v-icon>
      </v-btn>

      <v-spacer />

      <CalendarTypeControl :model="calendarTypeModel" />

      <v-spacer />

      <v-btn fab text small @click="$refs.calendar.next()">
        <v-icon>keyboard_arrow_right</v-icon>
      </v-btn>

      <v-progress-linear :active="loading" :indeterminate="loading" absolute bottom />
    </v-toolbar>

    <v-calendar
      id="calendar"
      :key="calendarVersion"
      ref="calendar"
      color="navbar"
      :style="`border-left:0; min-height:${calendarHeight}px;`"
      v-model="focus"
      :events="events"
      :event-color="(event) => event.color"
      :type="calendarTypeModel.type"
      :locale="userModule.getUser.language"
      :weekdays="weekdays"
      :event-more="10"
      @click:date="viewDay"
      @click:more="viewDay"
      @click:event="showEvent"
      @change="updateStartAndEnd"
    >
      <template v-slot:event="{ event }">
        <div class="pl-1">
          <strong>{{ user.formatTime(event.shift.time, 'Hmm') }}</strong>
          {{ event.name }}
        </div>
      </template>
    </v-calendar>

    <ShiftOverviewMenu :model="shiftOverviewModel" />

    <CreateShiftDialog
      v-if="canEdit"
      :date="focus"
      :categoryModel="categoryModel"
      :onCreated="calculateEvents"
    />
  </v-container>
</template>

<style lang="scss">
.v-event {
  margin-left: 3px;
}
.event-draft {
  background: repeating-linear-gradient(
    -45deg,
    #222,
    #222 5px,
    var(--v-accent-darken1) 5px,
    var(--v-accent-darken1) 10px
  );
  text-shadow: 2px 2px #222;
}
</style>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import moment from 'moment'
import i18n from '../../i18n'
import UserModule from '../../store/users'
import PersonModule from '../../store/persons'
import ProjectModule from '../../store/projects'
import CategoryModule from '../../store/categories'
import TeamModule from '../../store/teams'
import ShiftModule from '../../store/shifts'
import CreateShiftDialog from '../../components/dialogs/CreateShiftDialog.vue'
import ShiftOverviewMenu from '../../components/dialogs/ShiftOverviewMenu.vue'
import CategoriesControl from '../../components/controls/CategoriesControl.vue'
import CalendarTypeControl from '../../components/controls/CalendarTypeControl.vue'
import { Shift, Category, User } from '../../models'
import router from '../../router'

interface Event {
  name: string
  shift: Shift
  color: string
  start: string
  end: string
}

@Component({
  components: {
    CreateShiftDialog,
    ShiftOverviewMenu,
    CategoriesControl,
    CalendarTypeControl,
  },
})
export default class Calendar extends Vue {
  private userModule = getModule(UserModule, this.$store)
  private personModule = getModule(PersonModule, this.$store)
  private projectModule = getModule(ProjectModule, this.$store)
  private categoryModule = getModule(CategoryModule, this.$store)
  private teamModule = getModule(TeamModule, this.$store)
  private shiftModule = getModule(ShiftModule, this.$store)

  private loading = 0
  private calendarHeight = 0
  private focus = moment().format('YYYY-MM-DD')
  private events: Event[] = []
  private weekdays = [1, 2, 3, 4, 5, 6, 0]
  private user?: User
  private calendarVersion = 0

  private calendarTypeModel = { type: 'month' }
  private categoryModel: { value?: Category } = { value: undefined }
  private categoriesModel: { selected: Category[] } = { selected: [] }
  private shiftOverviewModel = { model: false, element: null, event: {} }

  private currentStart?: string
  private currentEnd?: string

  public get canView() {
    return this.personModule.getActiveRole?.calendarRead === true
  }

  private get canEdit() {
    const role = this.personModule.getActiveRole
    if (role) {
      return role.calendarWrite === true && role.eligibilities.some(x => x.shiftsWrite)
    }
  }

  private get getTitle() {
    switch (this.calendarTypeModel.type) {
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

  private calculateEvents() {
    const project = this.getProject()
    if (project) {
      // * when this gets re-evaluated, close any open shift overview menus
      this.shiftOverviewModel.model = false

      this.events = project.getCategories
        .filter(x => this.categoriesModel.selected.find(y => y.id === x.id))
        .reduce((accumulator: any, currentValue: Category) => {
          return accumulator.concat(currentValue.shifts)
        }, [])
        .map((x: Shift) => {
          const date = moment(x.date, 'YYYYMMDD').format('YYYY-MM-DD')
          const time = moment(x.time, 'Hmm').format(' HH:mm')
          const start = date + time
          const end = moment(start)
            .add(moment(x.duration, 'Hmm').format('H'), 'hours')
            .add(moment(x.duration, 'Hmm').format('mm'), 'minutes')
            .format('YYYY-MM-DD HH:mm')
          let color = 'event-draft'

          if (x.isPlanned) {
            color = 'navbar'
          } else if (x.isScheduled) {
            color = 'green darken-3'
          } else if (x.isSuspended) {
            color = 'red'
          } else if (x.isCalledOff) {
            color = 'grey'
          }

          return {
            name: x.category?.name,
            shift: x,
            color,
            start,
            end,
          } as Event
        })
    }
  }

  @Watch('personModule.getActivePerson')
  private async onPersonChanged(val: string, oldVal: string) {
    if (this.canView) {
      await this.init()
    }
  }

  @Watch('$route')
  private async onRouteChanged(val: string, oldVal: string) {
    if (this.canView) {
      await this.init()
    }
  }

  private async created() {
    if (this.canView) {
      await this.init()
    }
  }

  private async init() {
    this.loading++

    await this.categoryModule.loadCategories()

    if (this.canEdit) {
      await this.teamModule.loadTeams()
    }

    this.categoriesModel.selected = this.getProject()?.getCategories || []
    this.categoryModel.value = this.categoriesModel.selected[0]

    this.setCalendarHeight()

    this.user = this.userModule.getUser ?? undefined

    await this.loadMonth()

    this.loading--
  }

  private getProject() {
    return this.projectModule.getProjects.find(x => x.id === router.currentRoute.params.projectId)
  }

  private showEvent({ nativeEvent, event }: { nativeEvent: any; event: any }) {
    const open = () => {
      this.shiftOverviewModel.event = event
      this.shiftOverviewModel.element = nativeEvent.target
      setTimeout(() => (this.shiftOverviewModel.model = true), 10)
    }

    if (this.shiftOverviewModel.model) {
      this.shiftOverviewModel.model = false
      setTimeout(open, 10)
    } else {
      open()
    }

    nativeEvent.stopPropagation()
  }

  private viewDay({ date }: { date: string }) {
    this.focus = date
    this.calendarTypeModel.type = 'day'
  }

  private setToday() {
    this.focus = moment().format('YYYY-MM-DD')
  }

  private setCalendarHeight() {
    if (this.$vuetify.breakpoint.smAndDown) {
      this.calendarHeight = window.innerHeight - 112
    } else {
      this.calendarHeight = window.innerHeight - 128
    }
  }

  private async updateStartAndEnd(dates: { start: { date: string }; end: { date: string } }) {
    // load preceeding month for better pagination
    const start = moment(dates.start.date)
      .add(-41, 'days')
      .format('YYYY-MM-DD')

    // load following month for better pagination
    const end = moment(dates.end.date)
      .add(41, 'days')
      .format('YYYY-MM-DD')

    if (this.currentStart !== start || this.currentEnd !== end) {
      this.currentStart = start
      this.currentEnd = end

      await this.loadMonth()
    }
  }

  private async loadMonth() {
    this.loading++

    if (this.categoryModel.value && this.currentStart && this.currentEnd) {
      await this.shiftModule.loadShifts({
        start: this.currentStart.replace(/-/g, ''),
        end: this.currentEnd.replace(/-/g, ''),
      })

      this.calculateEvents()

      this.calendarVersion++
    }

    this.loading--
  }
}
</script>
